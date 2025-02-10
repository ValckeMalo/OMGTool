namespace OMG.Data.Mobs
{
    using MVProduction.CustomAttributes;
    using System.Collections.Generic;
    using OMG.Data.Mobs.Behaviour;
    using UnityEngine;
    using OMG.Card.Data;

    [CreateAssetMenu(fileName = "NewMobData", menuName = "Mobs/Data", order = 0)]
    public class MobData : ScriptableObject
    {
        [Title("Mob Data")]
        [SerializeField] private List<MobFightBehaviour> mobBehaviours;
        [SerializeField] private int experience;
        [SerializeField] private List<CardData> cardLootable;
        [SerializeField] private int baseHealth;
    }
}