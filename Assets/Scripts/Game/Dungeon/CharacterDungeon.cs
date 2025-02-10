namespace OMG.Game.Dungeon
{
    using MVProduction.CustomAttributes;
    using OMG.Data.Character;
    using UnityEngine;

    [System.Serializable]
    public class CharacterDungeon
    {
        [Title("Character Dungeon")]
        [SerializeField,ReadOnly] private CharacterManager charcterManager;
        [SerializeField,ReadOnly] private DungeonInventory dungeonInventory;
        [SerializeField,ReadOnly] private int health;
    }
}