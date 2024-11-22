namespace OMG.Battle
{
    using MaloProduction.CustomAttributes;

    using OMG.Card.UI;
    using OMG.Unit;
    using OMG.Unit.Player;
    using OMG.Battle.UI;
    using OMG.Battle.Data;

    using UnityEngine;
    using Unity.Behavior;

    public class PlayerBattleSystem : UnitBattleSystem
    {
        public static EventInitialize OnInitialize;
        public static EventUnitTurn OnUnitTurn;

        [SerializeField, ReadOnly] private Player player;
        [SerializeField, ReadOnly] private GameBoard gameBoard;

        [SerializeField, ReadOnly] private static int wakfu;
        [SerializeField, ReadOnly] private int wakfuUnlock;
        private const int maxWakfu = 6;
        private bool haveToRemovePadLock = false;
        private int WakfuRemain { get => wakfuUnlock - wakfu; }

        public virtual void Awake()
        {
            OnInitialize += Initialize;
            OnUnitTurn += UnitTurn;
        }
        public void Start() => HUDBattle.EndTurnButton.AddCallback(EndTurn);

        protected override bool Initialize(BattleData battleData, Blackboard blackboard)
        {
            InitializePlayer(battleData.PlayerData);
            InitialzeWakfu();
            InitializeGameBoard();

            return true;
        }
        protected override void UnitTurn(BattleData battleData)
        {
            TryClearArmor();
            HUDBattle.EndTurnButton.PlayerTurnButton();
            TryBreakPadLock();
            gameBoard.TrySpawnCards();
            TryDisableCardOnBoard(WakfuRemain);
        }

        #region Initialize
        private void InitializePlayer(IUnit unit)
        {
            if (unit == null)
            {
                return;
            }

            Player player = unit as Player;
            if (player == null)
            {
                return;
            }

            this.player = player;
        }
        private void InitialzeWakfu()
        {
            wakfu = 0;
            wakfuUnlock = 3;
            HUDBattle.PlayerWakfuGauge.ResetPadLock();
        }
        private void InitializeGameBoard()
        {
            gameBoard = new GameBoard(player.Deck, UseCard);
            gameBoard.ShuffleDeck();
        }
        #endregion

        #region Cards
        private bool UseCard(PlayableCard card)
        {
            if (card.IsDisable)
                return false;

            if (card.Type == CardType.Finisher)
            {
                gameBoard.DestroyFinishers();
                EndTurn();
                return true;
            }

            if (UseWakfu(card.Wakfu))
            {
                UseCardOnTarget(card.Data);
                gameBoard.RemoveCardOnBoard(card);
                TryDisableCardOnBoard(WakfuRemain);

                if (CanSpawnFinishers())
                {
                    SpawnFinishers();
                }

                return true;
            }

            return false;
        }
        private void UseCardOnTarget(CardData cardData)
        {
            if (cardData.target == Target.Player)
            {
                if (cardData.cardType == CardType.Defense)
                {
                    player.AddArmor(cardData.cardValue);
                }
            }
        }
        private bool CanSpawnFinishers()
        {
            return wakfu == maxWakfu;
        }
        private void SpawnFinishers()
        {
            gameBoard.SpawnFinishers();
            DisableEndTurn();
        }
        private void TryDisableCardOnBoard(int wakfuRemain) => gameBoard.TryDisableCardOnBoard(wakfuRemain);
        #endregion

        #region Wakfu
        public static void UpdatePreviewGauge(int wakfuCost)
        {
            HUDBattle.PlayerWakfuGauge.UpdatePreviewBar(wakfu + wakfuCost);
        }
        private void UnlockWakfu()
        {
            //if finisher spawn reset the wakfu bar to initial state
            if (wakfu == maxWakfu)
            {
                InitialzeWakfu();
            }
            else // else reset wakfu and try to unlock pad lock
            {
                wakfu = 0;
                wakfuUnlock++;

                //if the wakfu unlock havn't already exceed max possible break a pad lock
                if (wakfuUnlock <= maxWakfu)
                {
                    HUDBattle.PlayerWakfuGauge.BreakPadLock();
                    haveToRemovePadLock = true;
                }

                //to ensure that the wakfu usable don't exceed the max possible
                wakfuUnlock = Mathf.Min(wakfuUnlock, maxWakfu);
            }

            //reset the ui for the gauge and the preview gauge
            HUDBattle.PlayerWakfuGauge.ResetGauges();
        }
        private bool UseWakfu(int amount)
        {
            if (wakfu + amount > maxWakfu)
            {
                return false;
            }

            wakfu += amount;
            wakfu = Mathf.Max(wakfu, 0);
            UpdateWakfuGauge();

            return true;
        }
        private void UpdateWakfuGauge() => HUDBattle.PlayerWakfuGauge.UpdateWakfuBar(wakfu);
        private void TryBreakPadLock()
        {
            if (haveToRemovePadLock)
            {
                HUDBattle.PlayerWakfuGauge.RemovePadLock();
                haveToRemovePadLock = false;
            }
        }
        #endregion

        #region End Turn
        private void EndTurn()
        {
            UnlockWakfu();
            HUDBattle.EndTurnButton.MonstersTurnButton();
            BattleSystem.OnNextTurn?.Invoke(BattleState.Monsters);
        }
        private void DisableEndTurn()
        {
            HUDBattle.EndTurnButton.DisableButton();
        }
        #endregion

        #region Player
        private void TryClearArmor()
        {
            if (!player.Data.HaveStatus(OMG.Unit.Status.StatusType.Tenacite))
            {
                player.ClearArmor();
            }
        }
        #endregion
    }
}