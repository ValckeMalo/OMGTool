namespace OMG.Unit.Action
{
    using OMG.Unit;
    using UnityEngine;

    [CreateAssetMenu(fileName = "PiercingDamageAction", menuName = "Action/PiercingDamageAction")]
    public class PiercingDamageAction : UnitAction
    {
        [SerializeField] private int PiercingDamage = 0;

        public override void Execute(IUnit unit)
        {
            PiercingAttack(PiercingDamage,unit);
        }

        public override void Execute(IUnit[] units)
        {
            PiercingAttack(PiercingDamage, units);
        }

        public override int GetValue()
        {
            return PiercingDamage;
        }
    }
}