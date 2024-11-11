namespace MaloProduction.BehaviourTree.Blackboard
{
    using UnityEngine;
    using System.Collections.Generic;

    [CreateAssetMenu()]
    public class Blackboard : ScriptableObject
    {
        [SerializeField] private List<BlackboardValue> blackboard = new List<BlackboardValue>();
        public List<BlackboardValue> GetBlackboardValues { get => blackboard; }
    }

    public class BlackboardValue
    {
        public BlackboardValue(string key, object data)
        {
            this.data = data;
            this.key = key;
        }

        public string key = string.Empty;
        public object data = null;
    }
}