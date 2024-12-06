namespace OMG.Card.Data
{
    using MaloProduction.CustomAttributes;

    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewCardDeck", menuName = "Card/Deck", order = 0)]
    public class CardDeck : ScriptableObject
    {
        [Title("Deck Card")]
        public CardData[] cards = new CardData[14];
        [SerializeField] private List<CardData> finishers = new List<CardData>();

        [SerializeField] private List<CardData> curses = new List<CardData>();
        [SerializeField] private List<CardData> divine = new List<CardData>();

        public List<CardData> Finishers => finishers;
    }
}