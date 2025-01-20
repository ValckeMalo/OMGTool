namespace OMG.Unit.Action
{
    using MVProduction.CustomAttributes;
    using OMG.Unit;
    using UnityEngine;

    [CreateAssetMenu(fileName = "effectAction", menuName = "Action/Base/Effect")]
    public class EffectAction : UnitAction
    {
        [Title("State Action")]
        [SerializeField] private Status.StatusType Effect = Status.StatusType.Poison;

        public override void Execute(IUnit unit)
        {
            AddStatus(Value, Effect, unit);
        }

        public override void Execute(IUnit[] units)
        {
            AddStatus(Value, Effect, units);
        }
    }
}