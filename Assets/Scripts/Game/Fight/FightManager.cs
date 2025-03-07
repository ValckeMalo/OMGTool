namespace OMG.Game.Fight
{
    using MVProduction.CustomAttributes;

    using OMG.Data.Card;
    using OMG.Data.Character;
    using OMG.Data.Mobs;

    using OMG.Game.Dungeon;
    using OMG.Game.Fight.Cards;
    using OMG.Game.Fight.Entities;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;

    [System.Serializable]
    public class FightContext
    {
        public CharacterManager characterManager = null;
        public MobData[] mobsData = new MobData[3];
    }

    public class FightManager : MonoBehaviour
    {
        #region Singelton
        private static FightManager instance;
        public static FightManager Instance => instance;

        public void Awake()
        {
            if (instance == null)
                instance = this;
            else
                DestroyImmediate(gameObject);
        }
        #endregion

        [Title("Fight Manager")]
        [SerializeField] private FightContext context = null;
        private FightData fightData = null;
        private FightState currentState = FightState.None;
        public FightData FightData => fightData;

        [Header("UI")]
        [SerializeField] private FightUI fightUI;

        [Header("Entity")]
        [SerializeField] private Transform characterPosition;
        [SerializeField] private Transform[] mobsPosition = new Transform[3];
        public static Action OnMobDie;

        //Energy
        private int currentEnergy = 0;
        private int unlockEnergy = 3;
        private const int MaxEnergy = 6;
        private const int MinEnergy = 0;
        private const int MinUnlockEnergy = 3;
        private bool CanDoPerfect => currentEnergy == MaxEnergy;

        //Cards
        private FightDeck fightDeck = null;
        private bool needSecondCardToSelect = false;
        private FightCard secondCardSelected = null;
        private bool isFirstCardPlayed = true;

        #region Fight
        private void Start()
        {
            OnMobDie += UpdateFightProgress;
            InitializeFight();
        }
        private void InitializeFight()
        {
            //initialize card/deck
            fightDeck = new FightDeck(context.characterManager.Inventory.Deck);

            //Initialize fightData
            fightData = new FightData();
            fightData.InitializeData(SpawnCharacter(), SpawnMobs());

            //Initialize fight ui
            fightUI.Initialize(fightDeck);

            //subscribe to action on ui
            fightUI.OnRequestCharacterEndTurn += RequestEndCharacterTurn;
            fightUI.OnCancelCardSelect += CancelSelectCard;

            SetState(FightState.PlayerTurn);
        }
        private void SetState(FightState fightState)
        {
            currentState = fightState;

            switch (currentState)
            {
                case FightState.PlayerTurn:
                    UpdateFightProgress();
                    CharacterTurn();
                    break;

                case FightState.MobsTurn:
                    NewMobsTurn();
                    break;

                case FightState.Win:
                    RequestWin();
                    break;

                default:
                    Debug.LogError($"The state is not recognize or not implement : {currentState}");
                    break;
            }
        }
        private void UpdateFightProgress()
        {
            if (fightData.FightCharacterEntity == null || fightData.FightCharacterEntity.IsDying)
            {
                SetState(FightState.Lose);
                return;
            }

            if (fightData.AllMobs != null)
            {
                for (int i = 0; i < fightData.AllMobs.Count; i++)
                {
                    FightMobEntity fightMob = fightData.AllMobs[i];

                    if (fightMob == null) continue;

                    if (!fightMob.IsDying && fightMob.CanDie())
                    {
                        fightData.AllMobs.Remove(fightMob);
                        i--;
                        StartCoroutine(fightMob.RequestDie(
                        (FightMobEntity fightMob) =>
                        {
                            fightMob = null;
                            UpdateMobsPosition();
                        }
                        ));
                        continue;
                    }

                    return;
                }
            }

            SetState(FightState.Win);
        }
        #endregion

        #region Win
        private void RequestWin()
        {
            fightDeck.EndFight();
            StartCoroutine(AnimWin());
        }
        private IEnumerator AnimWin()
        {
            yield return new WaitForSeconds(0.5f);//let the mobs been killed
        }
        #endregion

        #region Mobs
        private List<FightMobEntity> SpawnMobs()
        {
            List<FightMobEntity> allMobsFightEntity = new List<FightMobEntity>();
            int i = 0;
            foreach (MobData mobData in context.mobsData)
            {
                FightMobEntity fightMobEntity = new FightMobEntity();
                fightMobEntity.InitializeMob(mobData.BaseHealth, mobData.BaseHealth, mobData, fightUI.SpawnMobEntityUI(mobsPosition[i].position));
                allMobsFightEntity.Add(fightMobEntity);
                fightMobEntity.SetPos(i);
                i++;
            }

            return allMobsFightEntity;
        }
        private void NewMobsTurn()
        {
            fightData.NewTurn();
            fightUI.StartMobsTurnButton();
            fightUI.PlayBannerTurnMobs();
            StartCoroutine(MobsTurn());
        }
        private IEnumerator MobsTurn()
        {
            foreach (FightMobEntity mob in fightData.AllMobs)
            {
                if (mob == null)
                    continue;

                if (mob.HasStatus(StatusType.Poison))
                {
                    mob.ApplyPoison();
                    yield return new WaitForSeconds(1.0f);
                }

                mob.NewTurn();

                yield return mob.PlayNextAction();
            }

            EndMobsTurn();
        }
        private void EndMobsTurn()
        {
            //search the next action perform by the mobs and update ui
            foreach (FightMobEntity mob in fightData.AllMobs)
            {
                if (mob == null)
                    continue;

                mob.SearchNextAction();
            }

            SetState(FightState.PlayerTurn);
        }

        private void SwitchMobPos(FightMobEntity fightMob, int newPos)
        {
            fightMob.UpdatePos(fightUI.WorldTofightCanvas(mobsPosition[newPos].position), newPos);
        }
        private void UpdateMobsPosition()
        {
            int mobPos = 0;
            foreach (FightMobEntity fightMob in fightData.AllMobs)
            {
                if (fightMob == null) continue;

                if (fightMob.Pos != mobPos)
                {
                    SwitchMobPos(fightMob, mobPos);
                }

                mobPos++;
            }
        }
        #endregion

        #region Character
        private FightCharacterEntity SpawnCharacter()
        {
            //Spawn Character
            DungeonCharacter dungeonCharacter = new DungeonCharacter(context.characterManager);
            FightCharacterEntity fightCharacterEntity = new FightCharacterEntity();
            fightCharacterEntity.InitializeCharacter(dungeonCharacter, fightUI.SpawnCharacterEntityUI(characterPosition.position));

            return fightCharacterEntity;
        }
        private void CharacterTurn()
        {
            fightDeck.NewTurn();//draw if possible next card
            fightUI.StartCharacterTurnButton(fightData.TurnCount);
            fightUI.PlayBannerTurnCharacter();
            fightUI.SetCardStateOnEnergy(unlockEnergy - currentEnergy);

            if (fightData.FightCharacterEntity != null)
                fightData.FightCharacterEntity.NewTurn(); //update the entity
        }
        public void RequestEndCharacterTurn()
        {
            isFirstCardPlayed = false;
            needSecondCardToSelect = false;
            secondCardSelected = null;

            if (CanDoPerfect)
            {
                fightDeck.RemovePerfectCardInHand();
            }

            EndTurnEnergy();//reset the energy to the base status / unlock new step ...

            SetState(FightState.MobsTurn);
        }
        #endregion

        #region Energy
        private void EndTurnEnergy()
        {
            if (CanDoPerfect)
            {
                fightUI.TogglePerfectEnergyUI(false);
                unlockEnergy = MinUnlockEnergy;
                fightUI.ResetPadlock();
            }
            else if (unlockEnergy < MaxEnergy)
            {
                unlockEnergy = Mathf.Min(unlockEnergy + 1, MaxEnergy);
                fightUI.RemovePadlock(Mathf.Abs(MaxEnergy - unlockEnergy - 2));//TODO break and in the start of the new turn remove
            }

            currentEnergy = MinEnergy;

            fightUI.UpdateCurrentEnergyUI(currentEnergy);
            fightUI.UpdatePreviewEnergyUI(currentEnergy);
        }
        private bool CanAddEnergy(int energyToAdd)
        {
            return (currentEnergy + energyToAdd <= unlockEnergy);
        }
        private void AddEnergy(int energyToAdd)
        {
            currentEnergy = Mathf.Clamp(currentEnergy + energyToAdd, 0, MaxEnergy);
            fightUI.UpdateCurrentEnergyUI(currentEnergy);
            fightUI.SetCardStateOnEnergy(unlockEnergy - currentEnergy);
        }
        #endregion

        #region Cards
        public void CardOver(FightCard fightCard)
        {
            if (!needSecondCardToSelect)
                fightUI.UpdatePreviewEnergyUI(currentEnergy + fightCard.Energy);
        }
        public void CardExit(FightCard fightCard)
        {
            if (!needSecondCardToSelect)
                fightUI.UpdatePreviewEnergyUI(currentEnergy);
        }

        public void TryUseCard(FightCard fightCard)
        {
            if (needSecondCardToSelect)
            {
                secondCardSelected = fightCard;
                return;
            }

            if (CanAddEnergy(fightCard.Energy))
            {
                if (fightCard.NeedAnotherCard)
                {
                    secondCardSelected = null;
                    needSecondCardToSelect = true;
                    StartCoroutine(SelectCard(fightCard));
                    return;
                }

                AddEnergy(fightCard.Energy);
                UseCard(fightCard);

                if (fightCard.IsPerfectCard)
                {
                    RequestEndCharacterTurn();
                    return;
                }

                if (CanDoPerfect)
                {
                    RequestPerfect();
                }
            }
        }
        private IEnumerator SelectCard(FightCard fightCard)
        {
            fightUI.ShowCardSelectUI(fightCard);

            if (fightCard.CardType == CardBackground.Boost)
            {
                fightUI.SetSelectableBoostableCard();
            }
            else if (fightCard.NeedSacrifice)
            {
                fightUI.SetSacrificialbeCard();
            }

            yield return new WaitUntil(() =>
            {
                if (secondCardSelected != null)
                {
                    if (secondCardSelected == fightCard)
                    {
                        secondCardSelected = null;
                        return false;
                    }

                    return true;
                }

                return false;
            });

            AddEnergy(fightCard.Energy);

            if (fightCard.CardType == CardBackground.Boost)
            {
                fightDeck.BoostFightCardInHand(secondCardSelected, fightCard.Value);
                FightCardProcess.ProcessOnlySpells(fightCard, isFirstCardPlayed, fightData, fightDeck);
                fightDeck.RemoveCardInHand(fightCard);
            }
            else if (fightCard.NeedSacrifice)
            {
                fightDeck.RemoveCardInHand(secondCardSelected);
                secondCardSelected = null;

                UseCard(fightCard);
            }
            else
            {
                Debug.LogError($"The first card does not seems to boost the other card or need a sacrifice {fightCard} & {secondCardSelected}");
            }

            fightUI.HideCardSelectUI();
            needSecondCardToSelect = false;

            if (CanDoPerfect)
            {
                RequestPerfect();
            }
        }
        private void UseCard(FightCard fightCard)
        {
            FightCardProcess.ProcessCard(fightCard, isFirstCardPlayed, fightData, fightDeck);
            isFirstCardPlayed = false;

            fightDeck.RemoveCardInHand(fightCard);
        }
        private void CancelSelectCard()
        {
            secondCardSelected = null;
            needSecondCardToSelect = false;

            fightUI.HideCardSelectUI();
            fightUI.SetCardStateOnEnergy(unlockEnergy - currentEnergy);
        }

        private void RequestPerfect()
        {
            fightDeck.DrawPerfectCard();
            fightUI.TogglePerfectEnergyUI(true);
            fightUI.SetPerfectMode();
        }
        #endregion
    }
}

//TODO Make All Todo in code

//TODO Made Mobs Creator Tool
//TODO Made MobAction Creator Tool
//TODO Made Deck Creator Tool
//TODO Made Character Creator Tool

//TODO Debug all

//TODO Add the Runes Features