namespace OMG.Data.Mobs
{
    using MVProduction.CustomAttributes;
    using System.Collections.Generic;
    using OMG.Data.Mobs.Behaviour;
    using UnityEngine;
    using OMG.Data.Card;

    [CreateAssetMenu(fileName = "NewMobData", menuName = "Mobs/Data", order = 0)]
    public class MobData : ScriptableObject
    {
        [Title("Mob Data")]
        [SerializeField] private List<MobFightBehaviour> mobBehaviours = new List<MobFightBehaviour>();
        [SerializeField] private int experience = 0;
        [SerializeField] private List<CardData> cardLootable = new List<CardData>();
        [SerializeField] private int baseHealth = 0;
        [SerializeField] public string entityName = string.Empty;

        public List<MobFightBehaviour> MobFightBehaviourList { get => mobBehaviours; set => mobBehaviours = value; }

        //the setter is Only Use for Editor Tools Do not touch it
        public int BaseHealth { get => baseHealth; set => baseHealth = value; }
        public int Experience { get => experience; set => experience = value; }
    }
}