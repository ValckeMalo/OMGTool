namespace MaloProduction.BehaviourTree
{
    using UnityEngine;

    public class BehaviourTree : ScriptableObject
    {
        private Node root;

        public Node.Status Update()
        {
            return root.Update();
        }
    }
}