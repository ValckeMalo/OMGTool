namespace OMG.Unit.Action
{
    using OMG.Unit;
    using UnityEngine;

    [CreateAssetMenu(fileName = "HealAction", menuName = "Action/HealAction")]
    public class HealAction : UnitAction
    {
        public int Health = 0;

        public override void Execute(IUnit unit)
        {
            Heal(Health, unit);
        }

        public override void Execute(IUnit[] units)
        {
            Heal(Health, units);
        }
    }
}