using UnityEngine;
using UnityEditor;

namespace MaloProduction
{
    public partial class DeckBuilderTools : EditorWindow
    {
        private void UpdateSettings()
        {
            HeaderSettings();
            SettingsSpriteField();
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
                if (GUILayout.Button("Back",
                    GUILayout.Height(50),
                    GUILayout.Width(50)))
                {
                    ChangeState(WindowState.ManageCard);
                }
            }
        }

        private void SettingsSpriteField()
        {
            GUILayout.BeginHorizontal(EditorStyles.helpBox, GUILayout.ExpandHeight(true));
            int index = 0;

            for (int j = 0; j < 2; j++)
            {
                GUILayout.FlexibleSpace();

                for (int i = 0; i < 4; i++)
                {
                    if (index < 7)
                    {
                        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                        {
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                GUILayout.FlexibleSpace();

                                cardOptions.collectionBgCardTexture[index].bgCardTexture = (Texture2D)EditorGUILayout.ObjectField(cardOptions.collectionBgCardTexture[index].bgCardTexture, typeof(Texture2D), false,
                                    GUILayout.MinHeight(190),
                                    GUILayout.MaxHeight(200),
                                    GUILayout.Height(190),
                                    GUILayout.MinWidth(140),
                                    GUILayout.MaxWidth(150),
                                    GUILayout.Width(140));

                                GUILayout.FlexibleSpace();
                            }

                            EditorGUILayout.LabelField(cardOptions.collectionBgCardTexture[index].type.ToString() + " Card", new GUIStyle(EditorStyles.boldLabel) { fontSize = 14, alignment = TextAnchor.MiddleCenter });
                        }
                        index++;
                    }

                    GUILayout.FlexibleSpace();
                }

                if (j == 0)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox, GUILayout.ExpandHeight(true));
                }
            }

            GUILayout.EndHorizontal();
        }
    }
}