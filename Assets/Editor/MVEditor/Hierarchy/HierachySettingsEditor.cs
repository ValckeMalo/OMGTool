namespace MVProduction.Hierarchy
{
    using System;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(HierarchySettings))]
    public class HierachySettingsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (target != null)
            {
                serializedObject.Update();

                HierarchySettings settings = serializedObject.targetObject as HierarchySettings;
                if (settings != null)
                {
                    if (GUILayout.Button("Refresh"))
                        settings.UpdateBlackListIcons();

                    if (settings.BlacklistIcons != null && settings.BlacklistIcons.Count > 0)
                    {
                        const int maxColumns = 2;
                        int currentColumn = 0;

                        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                        {
                            GUILayout.BeginHorizontal();
                            foreach (var f in settings.BlacklistIcons)
                            {
                                if (f.Name == string.Empty) continue;

                                GUI.backgroundColor = f.Disabled ? Color.red : Color.green;
                                using (new GUILayout.VerticalScope())
                                {
                                    Type type = Type.GetType(f.Name);
                                    using (new GUILayout.HorizontalScope())
                                    {
                                        GUILayout.FlexibleSpace();
                                        if (GUILayout.Button(EditorGUIUtility.ObjectContent(null, type).image, GUILayout.Width(50), GUILayout.Height(50)))
                                            f.Disabled = !f.Disabled;
                                        GUILayout.FlexibleSpace();
                                    }
                                    EditorGUILayout.LabelField(type.Name, new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleCenter });
                                }

                                currentColumn++;
                                if (currentColumn >= maxColumns)
                                {
                                    GUILayout.EndHorizontal();
                                    GUILayout.BeginHorizontal();
                                    currentColumn = 0;
                                }
                            }
                            GUILayout.EndHorizontal();
                        }

                        GUI.backgroundColor = Color.white;
                    }
                }

                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}