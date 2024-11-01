namespace OMG.Unit.Monster.Brain.Tree
{
    public class BranchNode : Node
    {
        private Node children;
        private System.Func<IUnit, IUnit, IUnit[], bool> condition;

        public BranchNode(Node children, System.Func<IUnit, IUnit, IUnit[], bool> condition)
        {
            this.children = children;
            this.condition = condition;
        }

        public override NodeResult Evaluate(IUnit player, IUnit monster, IUnit[] monsters)
        {
            if (children == null || condition == null)
                return NodeResult.Failure;

            if (condition(player, monster, monsters))
            {
                return children.Evaluate(player, monster, monsters);
            }

            return NodeResult.Failure;
        }
    }
}