namespace OMG.Data.Character
{
    using MVProduction.CustomAttributes;
    using UnityEngine;

    public class CharacterData : ScriptableObject
    {
        [Title("Character Data")]
        [SerializeField] private int baseHealth;
        [SerializeField] private int maxHealth;
    
		public int BaseHealth => baseHealth;
		public int MaxHealth => maxHealth;
	}
}