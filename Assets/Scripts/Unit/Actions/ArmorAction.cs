namespace OMG.Unit.Action
{
    using OMG.Unit;
    using UnityEngine;

    [CreateAssetMenu(fileName = "ArmorAction", menuName = "Action/ArmorAction")]
    public class ArmorAction : UnitAction
    {
        public int Armor = 0;
        public override void Execute(IUnit unit)
        {
            AddArmor(Armor, unit);
        }

        public override void Execute(IUnit[] units)
        {
            AddArmor(Armor, units);
        }

        public override int GetValue()
        {
            return Armor;
        }
    }
}