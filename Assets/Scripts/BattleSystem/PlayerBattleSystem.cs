namespace OMG.Battle
{
    using MaloProduction.CustomAttributes;

    using OMG.Card.Data;
    using OMG.Card.UI;
    using OMG.Unit;
    using OMG.Unit.Player;
    using OMG.Battle.UI;
    using OMG.Battle.Data;

    using System.Collections.Generic;
    using UnityEngine;
    using Unity.Behavior;

    public class PlayerBattleSystem : UnitBattleSystem
    {
        #region Class
        #region Game Board
        [System.Serializable]
        private class GameBoard
        {
            [Header("Deck")]
            [SerializeField] private CardDeck deck;

            [Header("Deck shuffle")]
            [SerializeField] private CardData[] deckShuffle;
            [SerializeField] private List<CardData> discardCard = new List<CardData>();
            [SerializeField] private int indexShuffle = 0;

            [Header("Card On Board")]
            [SerializeField] private List<PlayableCard> cardsOnBoard = new List<PlayableCard>();

            public int CountCardOnBoard { get => cardsOnBoard.Count; }

            public bool IsDeckNull { get => deck == null; }

            public GameBoard(CardDeck deck)
            {
                this.deck = deck;
            }

            /// <summary>
            /// Return the next card to come in the shuffle array
            /// Return null if none Card is inside
            /// </summary>
            /// <returns></returns>
            public CardData GetNextCard()
            {
                indexShuffle = (indexShuffle + 1) % deckShuffle.Length;
                int indexEntry = indexShuffle;

                if (deckShuffle == null || deckShuffle.Length <= 0)
                {
                    Debug.LogError($"The deck Shuffle doesn't exist");
                    return null;
                }

                CardData nextCard = deckShuffle[indexShuffle];
                bool hasRemainCard = true;

                while (nextCard == null && hasRemainCard)
                {
                    indexShuffle = (indexShuffle + 1) % deckShuffle.Length;
                    nextCard = deckShuffle[indexShuffle];

                    if (indexShuffle == indexEntry)
                        hasRemainCard = false;
                }

                //if a card his found add it to the discard array
                //and nullify it in the shuffle array
                if (nextCard != null)
                {
                    discardCard.Add(nextCard);
                    deckShuffle[indexShuffle] = null;
                }

                return nextCard;
            }

            public void ShuffleDeck() => deckShuffle = deck.ShuffleCard();

            public bool AddCardOnBoard(PlayableCard playableCard)
            {
                if (playableCard == null || cardsOnBoard == null)
                {
                    return false;
                }

                cardsOnBoard.Add(playableCard);
                return true;
            }
            public bool RemoveCardOnBoard(PlayableCard playableCard)
            {
                if (playableCard == null || !cardsOnBoard.Contains(playableCard))
                    return false;

                cardsOnBoard.Remove(playableCard);
                CardData cardData = playableCard.Data;

                //TODO not working fine find a better way
                bool hasToBeRemoved = playableCard.Use();
                if (!hasToBeRemoved)
                    ReintroduceCard(cardData);

                return true;
            }
            public void TryDisableCardOnBoard(int wakfuRemain)
            {
                foreach (PlayableCard playableCard in cardsOnBoard)
                {
                    if (playableCard.Wakfu <= wakfuRemain)
                    {
                        playableCard.EnableCard();
                    }
                    else
                    {
                        playableCard.DisableCard();
                    }
                }
            }
            public void HideAllCard()
            {
                foreach (PlayableCard playableCard in cardsOnBoard)
                {
                    playableCard.HideCard();
                }
            }
            public void ShowAllCard()
            {
                foreach (PlayableCard playableCard in cardsOnBoard)
                {
                    playableCard.ShowCard();
                }
            }

            private void ReintroduceCard(CardData card)
            {
                discardCard.Remove(card);
                int entryIndex = indexShuffle;
                int previousIndex = indexShuffle;

                CardData previousCard = deckShuffle[previousIndex];

                bool noPlaces = false;
                while (!noPlaces && previousCard != null)
                {
                    previousIndex = (previousIndex - 1 + deckShuffle.Length) % deckShuffle.Length;
                    previousCard = deckShuffle[previousIndex];

                    if (entryIndex == previousIndex)
                        noPlaces = true;
                }

                if (!noPlaces)
                {
                    deckShuffle[previousIndex] = card;
                }
            }

            public List<CardData> Finishers { get => deck.Finishers; }
        }
        #endregion
        #endregion

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
        public void Start()
        {
            HUDBattle.EndTurnButton.AddCallback(EndTurn);
        }

        protected override bool Initialize(BattleData battleData, Blackboard blackboard)
        {
            InitializePlayer(battleData.PlayerData);
            InitialzeWakfu();
            InitializeGameBoard();

            return true;
        }
        protected override void UnitTurn(BattleData battleData)
        {
            HUDBattle.EndTurnButton.PlayerTurnButton();
            TryBreakPadLock();
            TrySpawnCards();
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
            gameBoard = new GameBoard(player.Deck);
            gameBoard.ShuffleDeck();
        }
        #endregion

        #region Cards
        private void TrySpawnCards()
        {
            if (gameBoard.IsDeckNull)
            {
                Debug.Log($"Player doesn't have a Deck in {GetType().Name}.");
                return;
            }

            SpawnCards();
        }
        private void SpawnCards()
        {
            while (gameBoard.CountCardOnBoard < 6)
            {
                CardData cardToSpawn = gameBoard.GetNextCard();

                if (cardToSpawn == null)
                {
                    return;
                }

                SpawnCard(cardToSpawn);
            }
        }
        private void SpawnCard(CardData cardToSpawn)
        {
            PlayableCard newPlayableCard = CardSpawner.OnSpawnCard?.Invoke(cardToSpawn);
            newPlayableCard.RegisterOnClick(() => UseCard(newPlayableCard));
            gameBoard.AddCardOnBoard(newPlayableCard);
        }
        private void UseCard(PlayableCard card)
        {
            if (card.IsDisable)
                return;

            if (card.Type == CardType.Finisher)
            {
                gameBoard.ShowAllCard();
                EndTurn();
                return;
            }

            if (UseWakfu(card.Wakfu))
            {
                gameBoard.RemoveCardOnBoard(card);
                TryDisableCardOnBoard(WakfuRemain);
                TrySpawnFinishers();
            }
        }
        private void TrySpawnFinishers()
        {
            if (wakfu == maxWakfu)
            {
                SpawnFinishers();
            }
        }
        private void SpawnFinishers()
        {
            gameBoard.HideAllCard();
            HUDBattle.EndTurnButton.DisableButton();

            foreach (CardData finisher in gameBoard.Finishers)
            {
                if (finisher == null)
                {
                    continue;
                }

                SpawnCard(finisher);
            }
        }
        private void TryDisableCardOnBoard(int wakfuRemain)
        {
            gameBoard.TryDisableCardOnBoard(wakfuRemain);
        }
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
        #endregion
    }
}