namespace OMG.Unit.Action
{
    using OMG.Unit;
    using UnityEngine;

    [CreateAssetMenu(fileName = "AttackAction", menuName = "Action/AttackAction")]
    public class AttackAction : UnitAction
    {
        public int Damage = 0;

        public override void Execute(IUnit unit)
        {
            Attack(Damage, unit);
        }

        public override void Execute(IUnit[] units)
        {
            Attack(Damage, units);
        }

        public override int GetValue()
        {
            return Damage;
        }
    }
}