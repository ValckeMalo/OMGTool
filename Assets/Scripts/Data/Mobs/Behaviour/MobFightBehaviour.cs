namespace OMG.Data.Mobs.Behaviour
{
    using UnityEngine;
    using MVProduction.CustomAttributes;

    using OMG.Data.Mobs.Actions;

    [System.Serializable]
    public class MobFightBehaviour
    {
        [Title("Mob Fight Behaviour")]
        [SerializeField] private FightCondition primaryFightCondition;
        [SerializeField] private ConditionOperator conditionOperator;
        [SerializeField] private FightCondition secondaryFightCondition;
        [SerializeField] private SimpleMobActionsList mobActionList;//TODO don't forget to change the type to MobActionsList

        public FightCondition PrimaryFightCondition { get => primaryFightCondition; }
        public ConditionOperator ConditionOperator { get => conditionOperator; }
        public FightCondition SecondaryFightCondition { get => secondaryFightCondition; }
        public MobActionsList MobActionList { get => mobActionList; }
    }
}