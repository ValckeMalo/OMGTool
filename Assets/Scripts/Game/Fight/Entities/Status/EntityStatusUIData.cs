namespace OMG.Game.Fight.Entities
{
    using UnityEngine;
    using System.Collections.Generic;

    using MVProduction.CustomAttributes;

    using System.Linq;
    using System;

    [CreateAssetMenu(menuName = "Unit/Status UI", fileName = "StatusUIData", order = 3)]
    public class EntityStatusUIData : ScriptableObject
    {
        [Serializable]
        private class StatusUIValue
        {
            public StatusType key = StatusType.Poison;
            public string title = string.Empty;
            public string description = string.Empty;
            public Sprite icon = null;
        }

        [Title("Status UI Data")]
        [SerializeField] private List<StatusUIValue> statusUIs = new List<StatusUIValue>();

        private StatusUIValue GetValueByKey(StatusType key)
        {
            return statusUIs.Where(current => current.key == key).FirstOrDefault();
        }
		
		public bool Contains(StatusType key)
		{
			return statusUIs.Any(current => current.key == key);
		}
		
		public Sprite GetIconByKey(StatusType key)
		{
			if (!Contains(key))
				return null;
			
			return GetValueByKey(key).icon;
		}
		
		public string GetTitleByKey(StatusType key)
		{
			if (!Contains(key))
				return null;
			
			return GetValueByKey(key).title;
		}
		
		public string GetDescriptionByKey(StatusType key)
		{
			if (!Contains(key))
				return null;
			
			return GetValueByKey(key).description;
		}

        #region Editor
        [Button("Check Unique Key")]
        public void CheckUniqueKey()
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
        #endregion
    }
}