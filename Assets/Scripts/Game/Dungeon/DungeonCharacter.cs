namespace OMG.Game.Dungeon
{
    using MVProduction.CustomAttributes;
    using OMG.Data.Character;
    using UnityEngine;

    [System.Serializable]
    public class DungeonCharacter
    {
        [Title("Character Dungeon")]
        [SerializeField] private CharacterManager charcterManager;
        [SerializeField] private DungeonInventory dungeonInventory;
        [SerializeField] private int health;
		
		public int Health => health;
		public int MaxHealth => charcterManager.MaxHealth;
		
		public DungeonCharacter(CharacterManager charcterManager)
		{
			this.charcterManager = charcterManager;
			health = charcterManager.BaseHealth;
		}
    }
}