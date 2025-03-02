namespace OMG.Data.Character
{
    using UnityEngine;
    using MVProduction.CustomAttributes;

    [System.Serializable]
    public class CharacterManager
    {
        [Title("Character Manager")]
        [SerializeField] private CharacterData characterData;
        [SerializeField] private CharacterInventory characerInventory;
        [SerializeField] private CharacterLevel characterLevel;
		
		public CharacterInventory Inventory => characerInventory;
		public int BaseHealth => characterData.BaseHealth;
		public int MaxHealth => characterData.MaxHealth;
    }
}