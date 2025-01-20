namespace OMG.Unit.Action
{
    using OMG.Unit;
    using UnityEngine;

    [CreateAssetMenu(fileName = "HealAction", menuName = "Action/Base/Heal")]
    public class HealAction : UnitAction
    {
        public override void Execute(IUnit unit)
        {
            Heal(Value, unit);
        }

        public override void Execute(IUnit[] units)
        {
            Heal(Value, units);
        }
    }
}