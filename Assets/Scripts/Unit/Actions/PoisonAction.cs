namespace OMG.Unit.Action
{
    using OMG.Unit;
    using UnityEngine;

    [CreateAssetMenu(fileName = "PoisonAction",menuName = "Action/PoisonAction")]
    public class PoisonAction : UnitAction
    {
        public override void Execute(IUnit unit)
        {
            AddStatus(Value,Status.StatusType.Poison,unit);
        }

        public override void Execute(IUnit[] units)
        {
            AddStatus(Value, Status.StatusType.Poison, units);
        }
    }
}