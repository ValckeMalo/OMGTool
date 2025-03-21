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

        public FightConditionType ConditionType { get => conditionType; set => conditionType = value; }
        public ComparisonOperator ComparisionOperator { get => comparisionOperator; set => comparisionOperator = value; }
        public StatusType SpecificStatus { get => specificStatus; set => specificStatus = value; }
        public float SpecificValue { get => specificValue; set => specificValue = value; }
        public FightPosition SpecificPosition { get => specificPosition; set => specificPosition = value; }
    }
}