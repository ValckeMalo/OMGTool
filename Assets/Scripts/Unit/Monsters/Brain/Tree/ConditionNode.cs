namespace OMG.Unit.Monster.Brain.Tree
{
    using OMG.Unit.Action;

    public class ConditionNode : Node
    {
        private System.Func<IUnit, IUnit, IUnit[], bool> condition;
        private UnitAction action;
        private System.Action<UnitAction> setter;

        public ConditionNode(System.Func<IUnit, IUnit, IUnit[], bool> condition, UnitAction action, System.Action<UnitAction> setter)
        {
            this.condition = condition;
            this.action = action;
            this.setter = setter;
        }

        public override NodeResult Evaluate(IUnit player, IUnit monster, IUnit[] monsters)
        {
            if (action == null || condition == null || setter == null)
                return NodeResult.Failure;

            if (condition(player, monster, monsters))
            {
                setter(action);
                return NodeResult.Sucess;
            }

            return NodeResult.Failure;
        }
    }
}