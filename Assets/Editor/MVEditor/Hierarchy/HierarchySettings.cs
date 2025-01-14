namespace MaloProduction.Hierarchy
{
    using System.Collections.Generic;
    using System;
    using UnityEngine;

    using System.Reflection;
    using System.Linq;

    [CreateAssetMenu(fileName = "HierarchySettings", menuName = "MV Editor/Hierarchy Settings")]
    internal class HierarchySettings : ScriptableObject
    {
        [Serializable]
        public class Separator
        {
            public string StartString = "-";
            public Color Color = Color.red;
        }

        public bool Enabled = true;
        public Separator[] Separators;

        [SerializeField] public List<BlackListIcons> BlacklistIcons = new List<BlackListIcons>();
        public List<BlackListIcons> BlackListedType
        {
            get
            {
                return BlacklistIcons.Where(entry => entry.Disabled).Select(t => t).ToList();
            }
        }

        [Serializable]
        public class BlackListIcons
        {
            public bool Disabled = false;
            public string Name = "";

            public BlackListIcons(Type componentType)
            {
                Disabled = false;
                Name = componentType.AssemblyQualifiedName;
            }
        }
        public void UpdateBlackListIcons()
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
                        return entry.Name != string.Empty && entry.Name == componentType.AssemblyQualifiedName;
                    }
                ))
                    BlacklistIcons.Add(new BlackListIcons(componentType));
            }
        }
    }
}