using MaloProduction.CustomAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OMG.Card.Data
{
    [CreateAssetMenu(fileName = "NewCardDeck", menuName = "Card/Deck", order = 0)]
    public class CardDeck : ScriptableObject
    {
        [Title("Deck Card")]
        [SerializeField] private CardData[] cards = new CardData[14];
        [SerializeField] private CardData[] finishers = new CardData[5];

        [SerializeField] private List<CardData> curses = new List<CardData>();
        [SerializeField] private List<CardData> divine = new List<CardData>();

        public CardData[] ShuffleCard()
        {
            return AllCardPlayable().OrderBy(Card => Random.value).ToArray();
        }

        private CardData[] AllCardPlayable()
        {
            int nbCard = cards.Length + curses.Count + divine.Count;
            CardData[] allCards = new CardData[nbCard];

            int indexAllCards = 0;

            for (int i = 0; i < cards.Length; i++)
            {
                allCards[indexAllCards] = cards[i];
                indexAllCards++;
            }

            for (int i = 0; i < curses.Count; i++)
            {
                allCards[indexAllCards] = curses[i];
                indexAllCards++;
            }

            for (int i = 0; i < divine.Count; i++)
            {
                allCards[indexAllCards] = divine[i];
                indexAllCards++;
            }

            return allCards;
        }

        public bool RemoveCurseCard(CardData curseToRemove)
        {
            if (curseToRemove == null)
                return false;

            if (curses.Count <= 0)
                return false;

            if (!curses.Contains(curseToRemove))
                return false;

            curses.Remove(curseToRemove);
            return true;
        }

        public CardData[] Finishers { get => finishers; }
    }
}