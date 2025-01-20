namespace OMG.Unit.Action
{
    using OMG.Unit;
    using UnityEngine;

    [CreateAssetMenu(fileName = "AttackAction", menuName = "Action/Base/Attack")]
    public class AttackAction : UnitAction
    {
        public override void Execute(IUnit unit)
        {
            Attack(Value, unit);
        }

        public override void Execute(IUnit[] units)
        {
            Attack(Value, units);
        }
    }
}