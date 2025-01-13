namespace OMG.Unit.Action
{
    using OMG.Unit;
    using UnityEngine;

    [CreateAssetMenu(fileName = "PiercingDamageAction", menuName = "Action/PiercingDamageAction")]
    public class PiercingDamageAction : UnitAction
    {
        public override void Execute(IUnit unit)
        {
            PiercingAttack(Value,unit);
        }

        public override void Execute(IUnit[] units)
        {
            PiercingAttack(Value, units);
        }
    }
}