namespace OMG.Battle.Manager
{
    using OMG.Card.Data;

    using UnityEngine;

    using System.Collections.Generic;
    using System.Linq;
    using MaloProduction.CustomAttributes;

    [System.Serializable]
    public class CardDeckManager
    {
        [Title("Card Deck Manager")]
        private CardDeck deck;
        [SerializeField] private List<CardData> deckShuffle;
        [SerializeField] private List<CardData> cardPlayed;
        private int indexShuffle = 0;

        public bool IsDeckEmpty => deck == null;
        public List<CardData> Finishers => deck.Finishers;

        public CardDeckManager(CardDeck deck)
        {
            this.deck = deck;

            indexShuffle = 0;

            cardPlayed = new List<CardData>();

            FillDeckShuffle();
            ShuffleDeck();
        }

        private void FillDeckShuffle()
        {
            deckShuffle = new List<CardData>();
            deckShuffle.AddRange(deck.cards);
        }

        private void ShuffleDeck()
        {
            deckShuffle.OrderBy(Card => Random.value);
        }

        public CardData GetNextCard()
        {
            indexShuffle = (indexShuffle + 1) % deckShuffle.Count;

            if (deckShuffle == null || deckShuffle.Count <= 0)
            {
                Debug.LogError($"Can't get a card the deck shuffle is equal to null or they have no cards inside.");
                return null;
            }

            CardData nextCard = deckShuffle[indexShuffle];
            int indexEntry = indexShuffle;

            while (nextCard == null)
            {
                indexShuffle = (indexShuffle + 1) % deckShuffle.Count;
                nextCard = deckShuffle[indexShuffle];

                if (indexShuffle == indexEntry)
                    break;
            }

            //if a card his found add it to the discard array
            //and nullify it in the shuffle array
            if (nextCard != null)
            {
                cardPlayed.Add(nextCard);
                deckShuffle[indexShuffle] = null;
            }

            return nextCard;
        }

        public void ReintroduceCard(CardData card)
        {
            cardPlayed.Remove(card);

            if (card.isEtheral || card.cardType == CardType.Divine || card.cardType == CardType.Curse) return;

            int entryIndex = indexShuffle;
            int previousIndex = indexShuffle;

            CardData previousCard = deckShuffle[previousIndex];

            bool noPlaces = false;
            while (!noPlaces && previousCard != null)
            {
                previousIndex = (previousIndex - 1 + deckShuffle.Count) % deckShuffle.Count;
                previousCard = deckShuffle[previousIndex];

                if (entryIndex == previousIndex)
                    noPlaces = true;
            }

            if (!noPlaces)
            {
                deckShuffle[previousIndex] = card;
            }
        }

        public void AddACardInDrawPile(CardData card)
        {
            deckShuffle.Add(card);
        }
    }
}