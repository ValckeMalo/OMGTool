namespace MaloProduction.Hierarchy
{
    using System.Collections.Generic;
    using System;
    using UnityEngine;

    using System.Reflection;
    using System.Linq;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

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

            List<Type> allComponentsType = GetAllComponentTypes();

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
        private static List<Type> GetAllComponentTypes()
        {
            // Get all loaded assemblies
            Assembly[] allAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            // Collect all subclasses of Component and UIBehaviour
            List<Type> allComponentsType = new List<Type>();
            foreach (var assembly in allAssemblies)
            {
                try
                {
                    allComponentsType.AddRange(
                        assembly.GetTypes()
                            .Where(t => t.IsSubclassOf(typeof(Component)) && !t.IsAbstract)
                    );
                }
                catch (ReflectionTypeLoadException ex)
                {
                    // Handle types that cannot be loaded
                    Debug.LogWarning($"Failed to load types from assembly {assembly.FullName}: {ex}");
                }
            }

            return allComponentsType.Distinct().ToList(); // Ensure no duplicates
        }
    }
}