namespace MaloProduction.Hierachy
{
    using System.Collections.Generic;
    using System;
    using UnityEngine;
    using UnityEngine.UI;
    using System.Reflection;
    using MaloProduction.CustomAttributes;
    using System.Linq;

    [CreateAssetMenu(fileName = "HierarchySettings", menuName = "MV Editor/Hierarchy Settings")]
    public class HierarchySettings : ScriptableObject
    {
        [Serializable]
        public class Separator
        {
            public string StartString = "-";
            public Color Color = Color.red;
        }

        public bool Enabled = true;
        public Separator[] Separators;

        [SerializeField] private List<BlackListIcons> BlacklistIcons = new List<BlackListIcons>();
        public List<BlackListIcons> BlackListedIcons => BlacklistIcons.Where(entry => entry.IsBlackListed).Select(t => t).ToList();

        [Serializable]
        public class BlackListIcons
        {
            [SerializeField] private bool disabled = false;
            [SerializeField,ReadOnly] private string name = "";
            [SerializeField] private Type type = null;

            public Type Type => type;
            public bool IsBlackListed => disabled;

            public BlackListIcons(Type componentType)
            {
                type = componentType;
                disabled = false;
                name = type.Name;
            }
        }

        [Button("UpdateBlackListIcons")]
        private void UpdateBlackListIcons()
        {
            if (BlacklistIcons == null)
                BlacklistIcons = new List<BlackListIcons>();

            List<Type> allComponentsType = Assembly.GetAssembly(typeof(Component))
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(Component)) && !t.IsAbstract)
                .ToList();

            foreach (Type componentType in allComponentsType)
            {
                if (!BlacklistIcons.Any(entry => 
                    {
                        Debug.Log($"In List : {entry.Type.FullName} -- New Comp: {componentType.FullName}");
                        return entry.Type != null && entry.Type.FullName == componentType.FullName;
                    }))
                {
                    
                    BlacklistIcons.Add(new BlackListIcons(componentType));
                }
            }
        }
    }
}