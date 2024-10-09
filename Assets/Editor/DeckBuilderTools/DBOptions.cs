using UnityEngine;
using UnityEditor;

namespace MaloProduction
{
    public partial class DeckBuilderTools : EditorWindow
    {
        Texture2D text;

        private void UpdateOptionsMenu()
        {
            DrawHeaderOptionsMenu();
            DrawOptionSprite();
        }

        private void DrawHeaderOptionsMenu()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("Options",
                    new GUIStyle(EditorStyles.boldLabel) { fontSize = 20, alignment = TextAnchor.MiddleCenter },
                    GUILayout.ExpandWidth(true),
                    GUILayout.MinHeight(30),
                    GUILayout.MaxHeight(70),
                    GUILayout.Height(50));

                if (GUILayout.Button("B",
                    GUILayout.Height(50),
                    GUILayout.Width(50)))
                {
                    ChangeWindow(WindowState.MainMenu);
                }
            }
        }

        private void DrawOptionSprite()
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

                                soCardOptions.collectionBgCardTexture[index].bgCardTexture = (Texture2D)EditorGUILayout.ObjectField(soCardOptions.collectionBgCardTexture[index].bgCardTexture, typeof(Texture2D), false,
                                    GUILayout.MinHeight(190),
                                    GUILayout.MaxHeight(200),
                                    GUILayout.Height(190),
                                    GUILayout.MinWidth(140),
                                    GUILayout.MaxWidth(150),
                                    GUILayout.Width(140));

                                GUILayout.FlexibleSpace();
                            }

                            EditorGUILayout.LabelField(soCardOptions.collectionBgCardTexture[index].type.ToString() + " Card", new GUIStyle(EditorStyles.boldLabel) { fontSize = 14, alignment = TextAnchor.MiddleCenter });
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