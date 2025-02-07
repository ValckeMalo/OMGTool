namespace OMG.Data.Mobs.Behaviour
{
    using UnityEngine;
    using MVProduction.CustomAttributes;

    using OMG.Unit.Status;
    using OMG.Data.Utils;

    [System.Serializable]
    public class FightCondition
    {
        [Title("Fight Condition")]
        [SerializeField] private FightConditionType conditionType;
        [SerializeField] private ComparisonOperator comparisionOperator;
        [SerializeField] private StatusType specificStatus;
        [SerializeField] private float specificValue;
        [SerializeField] private FightPosition specificPosition;

        public FightConditionType ConditionType { get => conditionType; }
        public ComparisonOperator ComparisionOperator { get => comparisionOperator; }
        public StatusType SpecificStatus { get => specificStatus; }
        public float SpecificValue { get => specificValue; }
        public FightPosition SpecificPosition { get => specificPosition; }
    }
}