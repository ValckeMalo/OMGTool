namespace OMG.Unit.Monster.Brain.Tree
{
    using UnityEngine;

    public class SequencerNode : Node
    {
        [SerializeField] private Node[] childrens;

        public override NodeResult Evaluate(IUnit player, IUnit monster, IUnit[] monsters)
        {
            if (childrens == null)
                return NodeResult.Failure;

            foreach (Node children in childrens)
            {
                if (children == null)
                    continue;

                switch (children.Evaluate(player, monster, monsters))
                {
                    case NodeResult.Sucess:
                        return NodeResult.Sucess;

                    case NodeResult.Failure:
                        continue;

                    default:
                        continue;
                }
            }

            return NodeResult.Failure;
        }
    }
}