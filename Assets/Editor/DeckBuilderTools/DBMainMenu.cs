using UnityEditor;
using UnityEngine;

namespace MaloProduction
{
    public partial class DeckBuilderTools : EditorWindow
    {
        private void UpdateMainMenu()
        {
            DrawTitleMainMenu();
            GUILayout.FlexibleSpace();
            DrawButtonsMainMenu();
            GUILayout.FlexibleSpace();
            DrawOptionButton();
        }

        private void DrawTitleMainMenu()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("Deck Builder",
                    new GUIStyle(EditorStyles.boldLabel) { fontSize = 20, alignment = TextAnchor.MiddleCenter },
                    GUILayout.ExpandWidth(true),
                    GUILayout.MinHeight(30),
                    GUILayout.MaxHeight(70),
                    GUILayout.Height(50));
            }
        }

        private void DrawButtonsMainMenu()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true)))
            {
                GUILayout.FlexibleSpace();
                DrawAButtonMainMenu("Manage Cards", WindowState.ManageCardMenu);
                GUILayout.FlexibleSpace();
                DrawAButtonMainMenu("Create Cards", WindowState.CreateCard);
                GUILayout.FlexibleSpace();
            }
        }

        private void DrawAButtonMainMenu(string titleButton, WindowState nextWindow, Sprite buttonSprite = null)
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();

                    if (GUILayout.Button(titleButton,
                        GUILayout.MinHeight(125),
                        GUILayout.MaxHeight(200),
                        GUILayout.Height(175),
                        GUILayout.Width(150)))
                    {
                        ChangeWindow(nextWindow);
                    }

                    GUILayout.FlexibleSpace();
                }

                EditorGUILayout.LabelField(titleButton, new GUIStyle(GUI.skin.label) { alignment = TextAnchor.LowerCenter });
            }
        }

        private void DrawOptionButton()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
            {
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("O",

                GUILayout.MaxHeight(50),
                GUILayout.MinHeight(25),
                GUILayout.Height(35),

                GUILayout.MaxWidth(50),
                GUILayout.MinWidth(25),
                GUILayout.Width(35)))
                {
                    ChangeWindow(WindowState.OptionMenu);
                }
            }
        }
    }
}