namespace MaloProduction.BehaviourTree
{
    [System.Serializable]
    public abstract class Decorator : Node
    {
        protected Node child;

        public void AddChild(Node child)
        {
            this.child = child;
        }
    }
}