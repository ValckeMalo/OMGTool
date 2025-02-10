namespace OMG.Data.Character
{
    using UnityEngine;
    using Card.Data;
    using MVProduction.CustomAttributes;
    using System.Collections.Generic;

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