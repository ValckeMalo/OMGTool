namespace OMG.Battle
{
    using OMG.Card.Data;
    using OMG.Card.UI;

    using System;
    using System.Collections.Generic;

    using UnityEngine;

    [Serializable]
    public class GameBoard
    {
        [Header("Deck")]
        [SerializeField] private CardDeck deck;

        [Header("Deck shuffle")]
        [SerializeField] private CardData[] deckShuffle;
        [SerializeField] private List<CardData> discardCard = new List<CardData>();
        [SerializeField] private int indexShuffle = 0;

        [Header("Card On Board")]
        [SerializeField] private List<PlayableCard> cardsOnBoard = new List<PlayableCard>();
        private Func<PlayableCard, bool> useCardFunc;

        private int CountCardOnBoard { get => cardsOnBoard.Count; }
        private bool IsDeckNull { get => deck == null; }
        private List<CardData> Finishers { get => deck.Finishers; }

        public GameBoard(CardDeck deck, Func<PlayableCard, bool> UseCardFunc)
        {
            this.deck = deck;
            this.useCardFunc = UseCardFunc;
        }

        public void TrySpawnCards()
        {
            if (IsDeckNull)
            {
                Debug.Log($"Player doesn't have a Deck in {GetType().Name}.");
                return;
            }

            SpawnCards();
        }
        public void ShuffleDeck() => deckShuffle = deck.ShuffleCard();
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
        public void SpawnFinishers()
        {
            HideAllCard();

            foreach (CardData finisher in Finishers)
            {
                SpawnACard(finisher);
            }
        }
        public void DestroyFinishers()
        {
            foreach(PlayableCard card in cardsOnBoard)
            {
                if (card != null && card.Type == CardType.Finisher)
                {
                    card.Destroy();
                }
            }

            ShowAllCard();
        }

        private void SpawnCards()
        {
            while (CountCardOnBoard < 6)
            {
                CardData cardToSpawn = GetNextCard();

                if (cardToSpawn == null)
                {
                    return;
                }

                SpawnACard(cardToSpawn);
            }
        }
        private void SpawnACard(CardData cardToSpawn)
        {
            PlayableCard newPlayableCard = CardSpawner.OnSpawnCard?.Invoke(cardToSpawn);
            newPlayableCard.RegisterOnClick(() => useCardFunc(newPlayableCard));
            AddCardOnBoard(newPlayableCard);
        }
        private CardData GetNextCard()
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
        private bool AddCardOnBoard(PlayableCard playableCard)
        {
            if (playableCard == null || cardsOnBoard == null)
            {
                return false;
            }

            cardsOnBoard.Add(playableCard);
            return true;
        }
        private void HideAllCard()
        {
            foreach (PlayableCard playableCard in cardsOnBoard)
            {
                playableCard.HideCard();
            }
        }
        private void ShowAllCard()
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
    }
}