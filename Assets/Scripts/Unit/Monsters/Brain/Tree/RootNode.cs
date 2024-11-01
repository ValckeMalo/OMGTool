namespace OMG.Unit.Monster.Brain.Tree
{
    public class RootNode : Node
    {
        private Node children;

        public RootNode(Node children)
        {
            this.children = children;
        }

        public override NodeResult Evaluate(IUnit player, IUnit monster, IUnit[] monsters)
        {
            if (children == null)
                return NodeResult.Failure;

            return children.Evaluate(player, monster, monsters);
        }
    }
}