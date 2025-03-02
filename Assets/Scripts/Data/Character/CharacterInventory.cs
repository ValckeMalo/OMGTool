namespace OMG.Data.Character
{
    using UnityEngine;
    using MVProduction.CustomAttributes;
    using System.Collections.Generic;
    using OMG.Data.Card;

    [System.Serializable]
    public class CharacterInventory
    {
        [Title("Character Inventory")]
        [SerializeField] private CardDeck deck;
        [SerializeField] private List<CardData> finishersUnlock;
        [SerializeField] private int kamas;
		
		public CardDeck Deck => deck;
		public List<CardData> Finishers => finishersUnlock;
    }
}