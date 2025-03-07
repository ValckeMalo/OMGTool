namespace MVProduction.Hierarchy
{
    using UnityEngine;
    using UnityEditor;
    using static MVProduction.Hierarchy.HierarchySettings;

    [InitializeOnLoad]
    public class HierarchyIcons
    {
        private static HierarchySettings hierarchySettings;

        static HierarchyIcons()
        {
            hierarchySettings = LoadHierarchySettings();
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
        }

        private static void OnHierarchyGUI(int instanceID, Rect selectionRect)
        {
            if (hierarchySettings == null) return;
            if (!hierarchySettings.Enabled) return;

            GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

            if (obj == null) return;

            if (hierarchySettings.verticalLine != null)
            {
                if (hierarchySettings.verticalLine.ShowVerticalLine && obj.transform.parent != null)
                    DrawVerticalLine(selectionRect);
            }

            Component[] components = obj.GetComponents<Component>();
            if (components == null || components.Length == 0) return;

            DrawSeparator(obj, selectionRect);
            DrawIcons(components, selectionRect);
        }

        private static Texture GetComponentIcon(Component component)
        {
            GUIContent content = EditorGUIUtility.ObjectContent(null, component.GetType());
            return content.image;
        }

        private static HierarchySettings LoadHierarchySettings()
        {
            return AssetDatabase.LoadAssetAtPath<HierarchySettings>("Assets/Editor/MVEditor/Hierarchy/HierarchySettings.asset");
        }

        private static void DrawSeparator(GameObject obj, Rect selectionRect)
        {
            if (hierarchySettings.Separators == null || hierarchySettings.Separators.Length == 0) return;

            foreach (var separator in hierarchySettings.Separators)
                if (obj.name.Contains(separator.StartString))
                {
                    EditorGUI.DrawRect(selectionRect, separator.Color);
                }
        }

        private static void DrawIcons(Component[] components, Rect selectionRect)
        {
            float iconSpacing = 18f;
            float iconX = selectionRect.x + selectionRect.width - iconSpacing;

            foreach (Component component in components)
            {
                if (component == null) continue;
                if (IsBlackListComponent(component)) continue;

                Texture icon = GetComponentIcon(component);
                if (icon != null)
                {
                    Rect iconRect = new Rect(iconX, selectionRect.y, 16, 16);
                    GUI.DrawTexture(iconRect, icon);

                    iconX -= iconSpacing;
                }
            }
        }

        private static bool IsBlackListComponent(Component component)
        {
            if (component == null) return true;

            foreach (BlackListIcons blackListIcons in hierarchySettings.BlackListedType)
            {
                if (component.GetType().AssemblyQualifiedName == blackListIcons.Name) return true;
            }

            return false;
        }

        private static void DrawVerticalLine(Rect selectionRect)
        {
            Rect lineRect = new Rect(selectionRect.x + hierarchySettings.verticalLine.Offset, 
                                     selectionRect.y, hierarchySettings.verticalLine.Width, selectionRect.height);

            EditorGUI.DrawRect(lineRect, hierarchySettings.verticalLine.Color);
        }
    }
}