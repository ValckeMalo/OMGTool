namespace OMG.Unit.Action
{
    using OMG.Unit;
    using UnityEngine;

    [CreateAssetMenu(fileName = "PoisonAction",menuName = "Action/PoisonAction")]
    public class PoisonAction : UnitAction
    {
        public int NbTurn = 0;

        public override void Execute(IUnit unit)
        {
            AddStatus(NbTurn,Status.StatusType.Poison,unit);
        }

        public override void Execute(IUnit[] units)
        {
            AddStatus(NbTurn, Status.StatusType.Poison, units);
        }
    }
}