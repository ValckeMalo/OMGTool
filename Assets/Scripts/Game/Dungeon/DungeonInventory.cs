namespace OMG.Game.Dungeon
{
    using MVProduction.CustomAttributes;
    using UnityEngine;
    using System.Collections.Generic;
    using OMG.Card.Data;

    [System.Serializable]
    public class DungeonInventory
    {
        [Title("Dungeon Inventory")]
        [SerializeField, ReadOnly] private List<CardData> deck;
        //[SerializeField, ReadOnly] private List<RuneData> runes;
        //[SerializeField, ReadOnly] private ActivableItem activableItem;
    }
}