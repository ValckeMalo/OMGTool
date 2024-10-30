using OMG.Card.Data;
using OMG.Card.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMG.Battle
{
    public enum PlayerBattleState
    {
        Initialize,
        Action,
    }

    public class PlayerBattleSystem : MonoBehaviour
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

            /// <summary>
            /// Return the next card to come in the shuffle array
            /// Return null if none Card is inside
            /// </summary>
            /// <returns></returns>
            public CardData GetNextCard()
            {
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
        }
        #endregion
        #endregion

        #region Delegate
        public delegate void EventWakfuUse(int totalAmount, int maxWakfu);
        public static EventWakfuUse OnWakfuUse;
        #endregion

        [Header("Card Deck")]
        [SerializeField] private GameBoard gameBoard = new GameBoard();

        private int maxWakfu = 3;
        private int wakfuUsed = 0;
        private bool init = true;

        public void Awake() => BattleSystem.OnPlayerTurn += PlayerTurn;

        private void PlayerTurn(PlayerBattleState state)
        {
            switch (state)
            {
                case PlayerBattleState.Initialize:
                    StartCoroutine(Initialize());
                    break;

                case PlayerBattleState.Action:
                    StartCoroutine(Action());
                    break;

                default:
                    Debug.Log($"State not recognized : {state} in {GetType().Name}.");
                    break;
            }
        }

        private IEnumerator Initialize()
        {
            Debug.Log($"Preparing the player Turn");

            gameBoard.ShuffleDeck();
            UpdateWakfu();

            yield return new WaitForSeconds(0.5f);
            Debug.Log($"Preparation Finished");

            BattleSystem.OnPlayerTurn?.Invoke(PlayerBattleState.Action);
        }

        private void UpdateWakfu()
        {
            wakfuUsed = 0;
            if (!init)
            {
                maxWakfu++;
                maxWakfu = Mathf.Min(maxWakfu, 6);
            }
            else
            {
                init = false;
            }

            UpdateWakfuHUD();
        }

        private void SpawnCardFromDeck()
        {
            if (gameBoard.IsDeckNull)
            {
                Debug.Log($"Player doesn't have a Deck in {GetType().Name}.");
                return;
            }

            bool haveToExit = false;
            while (gameBoard.CountCardOnBoard < 6 && !haveToExit)
            {
                CardData cardToSpawn = gameBoard.GetNextCard();

                if (cardToSpawn == null)
                {
                    haveToExit = true;
                    continue;
                }

                PlayableCard newPlayableCard = CardSpawner.OnSpawnCard?.Invoke(cardToSpawn);
                newPlayableCard.RegisterOnClick(() => UseCard(newPlayableCard));
                gameBoard.AddCardOnBoard(newPlayableCard);
            }

            gameBoard.TryDisableCardOnBoard(maxWakfu - wakfuUsed);
        }

        private IEnumerator Action()
        {
            SpawnCardFromDeck();

            Debug.Log($"Wait For Action");
            yield return null;
        }

        private void UseCard(PlayableCard card)
        {
            if (card.IsDisable)
                return;

            wakfuUsed += card.Wakfu;
            wakfuUsed = Mathf.Max(wakfuUsed, 0);

            UpdateWakfuHUD();

            gameBoard.RemoveCardOnBoard(card);
            gameBoard.TryDisableCardOnBoard(maxWakfu - wakfuUsed);
        }

        private void UpdateWakfuHUD() => OnWakfuUse?.Invoke(wakfuUsed, maxWakfu);
    }
}