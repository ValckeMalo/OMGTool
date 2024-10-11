using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace MaloProduction
{
    public partial class DeckBuilderTools : EditorWindow
    {
        private string projectCardPath = "Assets/ScriptableObjects/Cards";
        private List<CardData> allCards = new List<CardData>();

        private void UpdateManageCard()
        {
            DrawHeaderManageCardMenu();

            if (allCards.Count > 0)
            {
                GridCardsButton();
            }
            else
            {
                NoneCardCreatedTitle();
            }
        }

        private void DrawHeaderManageCardMenu()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
            {
                //refresh all cards button
                if (GUILayout.Button("Refresh",
                    GUILayout.Height(50),
                    GUILayout.Width(50)
                    ))
                {
                    RefreshCardList();
                }

                //Title
                EditorGUILayout.LabelField("Manage Cards",
                    new GUIStyle(EditorStyles.boldLabel) { fontSize = 20, alignment = TextAnchor.MiddleCenter },
                    GUILayout.ExpandWidth(true),
                    GUILayout.MinHeight(30),
                    GUILayout.MaxHeight(70),
                    GUILayout.Height(50));

                //options button
                if (GUILayout.Button("Options",
                    GUILayout.Height(50),
                    GUILayout.Width(50)
                    ))
                {
                    ChangeState(WindowState.Settings);
                }
            }
        }

        private void RefreshCardList()
        {
            string[] assetPaths = AssetDatabase.FindAssets("t:CardData", new[] { projectCardPath });
            allCards.Clear();

            if (assetPaths.Length > 0)
            {
                foreach (string assetPath in assetPaths)
                {
                    string path = AssetDatabase.GUIDToAssetPath(assetPath);
                    CardData cardData = AssetDatabase.LoadAssetAtPath<CardData>(path);

                    if (cardData != null)
                    {
                        allCards.Add(cardData);
                    }
                }
            }
        }

        private void GridCardsButton()
        {
            float windowWidth = EditorGUIUtility.currentViewWidth;
            float buttonWidth = 85f;
            float margin = 10f;
            float remainingWidth = windowWidth;

            GUILayout.BeginHorizontal();

            for (int i = 0; i < allCards.Count; i++)
            {
                if (remainingWidth < buttonWidth + margin)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    remainingWidth = windowWidth;
                }

                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.Width(buttonWidth), GUILayout.Height(120)))
                {
                    PreviewCard(allCards[i], (int)buttonWidth);

                    EditorGUILayout.LabelField(allCards[i].cardName, new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleCenter }, GUILayout.Width(buttonWidth));
                }

                //draw invisible button on the top of the preview and name of the card
                Rect temp = GUILayoutUtility.GetLastRect();
                if (GUI.Button(temp, "", transparentButton))
                {
                    ChangeState(WindowState.ModifyCard);
                    UpdateSerializedCard(allCards[i], i);
                }

                remainingWidth -= buttonWidth + margin;
            }

            GUILayout.EndHorizontal();
        }

        private void NoneCardCreatedTitle()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true)))
            {
                EditorGUILayout.LabelField("None Card Created", new GUIStyle(GUI.skin.label)
                { alignment = TextAnchor.MiddleCenter, fontSize = 25, fontStyle = FontStyle.Bold },
                GUILayout.ExpandHeight(true),
                GUILayout.ExpandWidth(true));
            }
        }

        private void GetNextCard()
        {
            int nextIndex = indexCardToModify + 1;
            if (nextIndex >= allCards.Count) nextIndex = 0;

            UpdateSerializedCard(allCards[nextIndex], nextIndex);
        }

        private void GetPreviousCard()
        {
            int previousIndex = indexCardToModify - 1;
            if (previousIndex < 0) previousIndex = allCards.Count - 1;

            UpdateSerializedCard(allCards[previousIndex], previousIndex);
        }
    }
}