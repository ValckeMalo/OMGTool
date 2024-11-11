namespace OMG.Unit.Monster.Brain.Node
{
    using MaloProduction.BehaviourTree;

    public class StatCondition : Action
    {
        public enum UnitStat
        {
            Life,
            Armor,
        }

        public UnitData unit;
        public UnitStat stat = UnitStat.Life;
        public int value = 0;

        protected override Status OnStart()
        {
            if (unit == null)
            {
                return Status.Failure;
            }

            return Status.Running;
        }

        protected override void OnStop() { }

        protected override Status OnUpdate()
        {
            if (GetStat(stat) > value)
            {
                return child.Update();
            }

            return Status.Failure;
        }

        private int GetStat(UnitStat unitStat)
        {
            switch (unitStat)
            {
                case UnitStat.Life:
                    return unit.hp;

                case UnitStat.Armor:
                    return unit.armor;

                default:
                    return unit.hp;
            }
        }
    }
}