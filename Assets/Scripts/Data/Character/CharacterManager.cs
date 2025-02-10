namespace OMG.Data.Character
{
    using UnityEngine;
    using MVProduction.CustomAttributes;

    [System.Serializable]
    public class CharacterManager
    {
        [Title("Character Manager")]
        [SerializeField, ReadOnly] private MobData characerData;
        [SerializeField, ReadOnly] private CharacterInventory characerInventory;
        [SerializeField, ReadOnly] private CharacterLevel characterLevel;
    }
}