namespace OMG.Data.Card
{
    using MVProduction.CustomAttributes;

    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewCardDeck", menuName = "Card/Deck", order = 0)]
    public class CardDeck : ScriptableObject
    {
        [Title("Deck Card")]
        [SerializeField] private List<CardData> cards;
        [SerializeField] private List<CardData> finishers;

        public List<CardData> Cards => cards;
        public List<CardData> Finishers => finishers;
    }
}