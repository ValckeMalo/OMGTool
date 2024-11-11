namespace MaloProduction.BehaviourTree
{
    using System.Collections.Generic;
    using System.Linq;

    [System.Serializable]
    public abstract class Composite : Node
    {
        protected List<Node> children = new List<Node>();

        public void AddChild(params Node[] children)
        {
            this.children = children.ToList();
        }
    }
}