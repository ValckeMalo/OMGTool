namespace OMG.Data.Mobs.Behaviour
{
    using UnityEngine;
    using MVProduction.CustomAttributes;

    using OMG.Data.Utils;
    using OMG.Game.Fight.Entities;

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