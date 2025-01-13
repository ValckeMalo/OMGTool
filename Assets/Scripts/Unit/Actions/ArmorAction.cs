namespace OMG.Unit.Action
{
    using OMG.Unit;
    using UnityEngine;

    [CreateAssetMenu(fileName = "ArmorAction", menuName = "Action/ArmorAction")]
    public class ArmorAction : UnitAction
    {
        public override void Execute(IUnit unit)
        {
            AddArmor(Value, unit);
        }

        public override void Execute(IUnit[] units)
        {
            AddArmor(Value, units);
        }
    }
}