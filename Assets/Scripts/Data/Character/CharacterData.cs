namespace OMG.Data.Character
{
    using MVProduction.CustomAttributes;
    using OMG.Data.Mobs.Behaviour;
    using System.Collections.Generic;
    using UnityEngine;

    public class MobData : ScriptableObject
    {
        [Title("Character Data")]
        [SerializeField] private int baseHealth;
        [SerializeField] private int maxHealth;
        [SerializeField] private List<MobFightBehaviour> mobFightBehaviourList;

        public List<MobFightBehaviour> MobFightBehaviourList => mobFightBehaviourList;
    }
}