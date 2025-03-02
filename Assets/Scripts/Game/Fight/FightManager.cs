namespace OMG.Game.Fight
{
    using MVProduction.CustomAttributes;

    using OMG.Data.Card;
    using OMG.Data.Character;
    using OMG.Data.Mobs;

    using OMG.Game.Dungeon;
    using OMG.Game.Fight.Cards;
    using OMG.Game.Fight.Entities;

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

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
        private FightState previousState = FightState.None;
        public FightData FightData => fightData;

        [Header("UI")]
        [SerializeField] private FightUI fightUI;

        [Header("Entity")]
        [SerializeField] private Transform characterPosition;
        [SerializeField] private Transform[] mobsPosition = new Transform[3];
        [SerializeField] private GameObject characterFightEntityPrefab;
        [SerializeField] private GameObject mobFightEntityPrefab;

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

        private void Start()
        {
            InitializeFight();
        }
        private void InitializeFight()
        {
            //initialize card/deck
            fightDeck = new FightDeck(context.characterManager.Inventory.Deck);

            //Spawn entity
            List<FightMobEntity> allMobsFightEntity = new List<FightMobEntity>();
            int i = 0;
            foreach (MobData mobData in context.mobsData)
            {
                GameObject newMobs = Instantiate(mobFightEntityPrefab, mobsPosition[i].position, Quaternion.identity, null);
                FightMobEntityUI fightMobEntityUI = newMobs.GetComponent<FightMobEntityUI>();
                FightMobEntity fightMobEntity = new FightMobEntity();
                fightMobEntity.InitializeMob(mobData.BaseHealth, mobData.BaseHealth, mobData, fightMobEntityUI);
                allMobsFightEntity.Add(fightMobEntity);
                i++;
            }

            //Spawn Character
            DungeonCharacter dungeonCharacter = new DungeonCharacter(context.characterManager);
            GameObject characterObject = Instantiate(characterFightEntityPrefab, null);
            FightCharacterEntityUI fightCharacterEntityUI = characterObject.GetComponent<FightCharacterEntityUI>();
            FightCharacterEntity fightCharacterEntity = new FightCharacterEntity();
            fightCharacterEntity.InitializeCharacter(dungeonCharacter, fightCharacterEntityUI);
            
            //spawn the entities and their UI
            InitsMobs();
            InitCharacter();

            //Initialize fightData
            fightData = new FightData();
            fightData.InitializeData(fightCharacterEntity, allMobsFightEntity);

            fightUI.Initialize(fightDeck);

            SetState(FightState.PlayerTurn);
        }
        private void SetState(FightState fightState)
        {
            previousState = currentState;
            currentState = fightState;

            UpdateFightProgress();

            switch (currentState)
            {
                case FightState.PlayerTurn:
                    CharacterTurn();
                    break;

                case FightState.MobsTurn:
                    NewMobsTurn();
                    break;

                default:
                    Debug.LogError($"The state is not recognize or not implement : {currentState}");
                    break;
            }
        }
        private void UpdateFightProgress()
        {
            if (fightData.FightCharacterEntity == null || fightData.FightCharacterEntity.IsDying())
            {
                SetState(FightState.Lose);
                return;
            }

            if (fightData.AllMobs == null)
            {
                SetState(FightState.Win);
                return;
            }

            foreach (FightMobEntity fightMob in fightData.AllMobs)
            {
                if (fightMob == null || fightMob.IsDying())
                    continue;

                return;
            }

            SetState(FightState.Win);
        }

        #region Mobs
        private void InitsMobs()
        {

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
        #endregion

        #region Character
        private void InitCharacter()
        {
            
        }
        private void CharacterTurn()
        {
            fightDeck.NewTurn();//draw if possible next card
            fightUI.StartCharacterTurnButton(fightData.TurnCount);
            fightUI.PlayBannerTurnCharacter();

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
                unlockEnergy = MinUnlockEnergy;
                fightUI.ResetPadlock();
            }
            else if (unlockEnergy < MaxEnergy)
            {
                unlockEnergy = Mathf.Min(unlockEnergy + 1, MaxEnergy);
                fightUI.RemovePadlock(MaxEnergy - unlockEnergy);//TODO break and in the start of the new turn remove
            }

            currentEnergy = MinEnergy;
            unlockEnergy = MinUnlockEnergy;

            fightUI.UpdateCurrentEnergyUI(currentEnergy);
            fightUI.UpdatePreviewEnergyUI(currentEnergy);

            fightUI.OnRequestCharacterEndTurn += RequestEndCharacterTurn;
            fightUI.OnCancelCardSelect += CancelSelectCard;
        }
        public bool TryAddEnergy(int energyToAdd)
        {
            if (CanAddEnergy(energyToAdd))
            {
                AddEnergy(energyToAdd);
                return true;
            }

            return false;
        }
        private bool CanAddEnergy(int energyToAdd)
        {
            return (currentEnergy + energyToAdd <= unlockEnergy) && (currentEnergy + energyToAdd > 0);
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
            fightUI.UpdatePreviewEnergyUI(currentEnergy + fightCard.Energy);
        }
        public void CardExit(FightCard fightCard)
        {
            fightUI.UpdatePreviewEnergyUI(currentEnergy);
        }

        public void TryUseCard(FightCard fightCard)
        {
            if (needSecondCardToSelect)
            {
                secondCardSelected = fightCard;
                needSecondCardToSelect = false;
                return;
            }

            if (CanAddEnergy(fightCard.Energy))
            {
                if (fightCard.NeedAnotherCard)
                {
                    StartCoroutine(SelectCard(fightCard));
                    return;
                }

                AddEnergy(fightCard.Energy);
                UseCard(fightCard);

                if (CanDoPerfect)
                {
                    RequestPerfect();
                }
            }
        }
        private IEnumerator SelectCard(FightCard fightCard)
        {
            fightUI.ShowCardSelectUI();

            if (fightCard.CardType == CardBackground.Boost)
            {
                fightUI.SetSelectableBoostableCard();
            }
            else if (fightCard.NeedSacrifice)
            {
                fightUI.SetSacrificialbeCard();
            }

            yield return new WaitUntil(() => secondCardSelected != null);

            AddEnergy(fightCard.Energy);

            if (fightCard.CardType == CardBackground.Boost)
            {
                fightDeck.BoostFightCardInHand(secondCardSelected, fightCard.Value);
                FightCardProcess.ProcessOnlySpells(fightCard, isFirstCardPlayed, fightData, fightDeck);
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
            fightUI.SetPerfectMode();
        }
        #endregion
    }
}

//TODO les mobs n'ont pas leur place dans la list
//TODO les mobs ne peuvent pas changer de place lors de la mort d'un autre

//TODO Launch Unity and clean all Errors
//TODO Make All Todo in code if possible
//TODO Rename Class/Function that need it
//TODO Assign all values in the inspector

//TODO Clean Files in Directory
//TODO Debug new Fight System

//TODO Made Mobs Creator Tool
//TODO Made MobAction Creator Tool
//TODO Made Deck Creator Tool
//TODO Made Character Creator Tool

//TODO Debug all

//TODO Add the Runes Features