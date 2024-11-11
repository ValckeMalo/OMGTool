namespace MaloProduction.BehaviourTree
{
    [System.Serializable]
    public abstract class Action : Node
    {
        protected Node child;

        public void AddChild(Node child)
        {
            this.child = child;
        }
    }
}