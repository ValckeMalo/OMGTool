namespace OMG.Data.Character
{
    using UnityEngine;
    using Card.Data;
    using MVProduction.CustomAttributes;
    using System.Collections.Generic;

    [System.Serializable]
    public class CharacterInventory
    {
        [Title("Character Inventory")]
        [SerializeField, ReadOnly] private CardDeck deck;
        [SerializeField, ReadOnly] private List<CardData> finishersUnlock;
        [SerializeField, ReadOnly] private int kamas;
    }
}