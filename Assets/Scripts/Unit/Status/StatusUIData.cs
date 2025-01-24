namespace OMG.Unit.HUD
{
    using UnityEngine;
    using System.Collections.Generic;

    using MVProduction.CustomAttributes;

    using OMG.Unit.Status;
    using System.Linq;

    [CreateAssetMenu(menuName = "Unit/Status UI", fileName = "StatusUIData", order = 3)]
    public class StatusUIData : ScriptableObject
    {
        [System.Serializable]
        public class StatusUIValue
        {
            public StatusType key = StatusType.Poison;
            public string title = string.Empty;
            public string description = string.Empty;
            public Sprite icon = null;
        }

        [Title("Status UI Data")]
        [SerializeField] private List<StatusUIValue> statusUIs = new List<StatusUIValue>();

        public StatusUIValue GetValueByKey(StatusType key)
        {
            return statusUIs.Where(current => current.key == key).FirstOrDefault();
        }

        [Button("Check Unique Key")]
        private void CheckUniqueKey()
        {
            HashSet<StatusType> seenKeys = new HashSet<StatusType>();

            foreach (var status in statusUIs)
            {
                if (!seenKeys.Add(status.key))
                {
                    Debug.LogError($"Duplicate key found: {status.key}");
                }
            }

            seenKeys.Clear();
        }
    }
}