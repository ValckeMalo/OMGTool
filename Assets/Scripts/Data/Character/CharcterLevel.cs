namespace OMG.Data.Character
{
    using UnityEngine;
    using System.Collections.Generic;
    using MVProduction.CustomAttributes;

    [System.Serializable]
    public class CharacterLevel
    {
        [Title("Character Level")]
        [SerializeField, ReadOnly] private int level;
        [SerializeField, ReadOnly] private int experience;
        [SerializeField] private List<int> experienceTreshold;
        private const int MaxLevel = 25;
    }
}