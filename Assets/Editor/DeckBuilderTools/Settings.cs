namespace MaloProduction
{
    using UnityEngine;
    using UnityEditor;

    public partial class DeckBuilderTools : EditorWindow
    {
        private void UpdateSettings()
        {
            HeaderSettings();
            SettingsBody();
        }

        private void HeaderSettings()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
            {
                //Title
                EditorGUILayout.LabelField("Settings",
                    new GUIStyle(EditorStyles.boldLabel) { fontSize = 20, alignment = TextAnchor.MiddleCenter },
                    GUILayout.ExpandWidth(true),
                    GUILayout.MinHeight(30),
                    GUILayout.MaxHeight(70),
                    GUILayout.Height(50));

                //Button Back
                if (GUILayout.Button(BackTexture,
                    GUILayout.Height(50),
                    GUILayout.Width(50)))
                {
                    ChangeState(WindowState.ManageCard);
                }
            }
        }

        private void SettingsBody()
        {

            //all body
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
            {
                for (int i = 0; i < 2; i++)
                {
                    using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                    {
                        SettingsField();
                        SettingsField();
                        SettingsField();
                    }
                }
            }
        }

        private void SettingsField()
        {
            //box
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
            {
                //title
                EditorGUILayout.LabelField("Attack", new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleCenter }, GUILayout.ExpandWidth(true));

                //container textures + labels
                using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                {
                    //first texture (background)
                    using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                    {
                        EditorGUILayout.ObjectField(TrashCanTexture, typeof(Texture2D), false, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                        EditorGUILayout.LabelField("Background", new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleCenter }, GUILayout.MaxWidth(150f));
                    }

                    GUILayout.Space(5f);

                    //second texture (Icon)
                    using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                    {
                        EditorGUILayout.ObjectField(TrashCanTexture, typeof(Texture2D), false, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                        EditorGUILayout.LabelField("Icon", new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleCenter }, GUILayout.MaxWidth(150f));
                    }
                }
            }

        }
    }
}