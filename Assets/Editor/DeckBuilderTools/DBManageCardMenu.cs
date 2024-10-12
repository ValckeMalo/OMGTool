using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace MaloProduction
{
    public partial class DeckBuilderTools : EditorWindow
    {
        private enum Comparison
        {
            GreaterOrEqual,
            Equal,
            LessOrEqual,
        }

        private string projectCardPath = "Assets/ScriptableObjects/Cards";
        private List<CardData> allCards = new List<CardData>();
        private Filter[] filters = new Filter[4]
            {
                new Filter(new List<FilterElement>()
                {
                    new FilterElement("Wakfu Cost"),
                    new FilterElement(Comparison.Equal),
                    new FilterElement(0,0,6)
                }),

                new Filter(new List<FilterElement>()
                {
                    new FilterElement("Value Card"),
                    new FilterElement(Comparison.Equal),
                    new FilterElement(0),
                }),

                new Filter(new List<FilterElement>()
                {
                    new FilterElement("Target"),
                    new FilterElement(Target.FirstEnemy),
                }),

                new Filter(new List<FilterElement>()
                {
                    new FilterElement(Spells.Poison),
                    new FilterElement(Comparison.Equal),
                    new FilterElement(0),
                }),
            };

        //Filter variable
        private string nameFilter = string.Empty;
        private CardTypeFilter typeFilter = CardTypeFilter.None;

        private void UpdateManageCard()
        {
            HeaderManageCardMenu();
            ToolBarMangeCard();

            if (allCards.Count > 0)
            {
                GridCardsButton();
            }
            else
            {
                NoneCardCreatedTitle();
            }

            GUI.enabled = true;
        }

        private enum CardTypeFilter
        {
            Attack,
            Defense,
            Boost,
            Neutral,
            GodPositive,
            GodNegativ,
            Finisher,
            None,
        }

        private bool isCLicked = false;
        private bool toggle = false;
        //private Spells spells = Spells.Poison;
        //private int amountSpell = 0;

        private void ToolBarMangeCard()
        {
            //tool bar scope
            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.MaxHeight(50f)))
            {
                //Title
                using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandHeight(true)))
                {
                    EditorGUILayout.LabelField("Filter : ",
                        new GUIStyle(EditorStyles.boldLabel) { fontSize = 15, alignment = TextAnchor.MiddleLeft },
                        GUILayout.ExpandHeight(true),
                        GUILayout.Width(50f));
                }

                //name Filter
                using (new EditorGUILayout.VerticalScope())
                {
                    GUILayout.FlexibleSpace();
                    using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandHeight(true)))
                    {
                        EditorGUILayout.LabelField("Name : ", GUILayout.MaxWidth(50f));
                        nameFilter = EditorGUILayout.TextField(nameFilter, GUILayout.MaxWidth(150f));
                    }
                    GUILayout.FlexibleSpace();
                }

                //type filter
                using (new EditorGUILayout.VerticalScope())
                {
                    GUILayout.FlexibleSpace();

                    using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandHeight(true)))
                    {
                        EditorGUILayout.LabelField("Type : ", GUILayout.MaxWidth(50f));
                        typeFilter = (CardTypeFilter)EditorGUILayout.EnumPopup(typeFilter, GUILayout.MaxWidth(150f));
                    }
                    GUILayout.FlexibleSpace();
                }

                //other filter
                using (new EditorGUILayout.VerticalScope())
                {
                    GUILayout.FlexibleSpace();
                    using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandHeight(true)))
                    {
                        EditorGUILayout.LabelField("Other : ", GUILayout.MaxWidth(50f));

                        string label = "No Filters Selected";
                        if (toggle)
                        {
                            label = "One Filter Selected";
                        }

                        if (GUILayout.Button(label,
                                             new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter, fontSize = 10, fontStyle = FontStyle.Italic },
                                             GUILayout.MaxWidth(300f)))
                        {
                            isCLicked = isCLicked.Invert();
                        }

                    }

                    if (isCLicked)
                    {
                        Rect lastRect = GUILayoutUtility.GetLastRect();
                        float yMarginDropdown = 5f;
                        float heightDropdown = 100f;
                        Vector2 dropdownPosition = new Vector2(lastRect.x, lastRect.y + lastRect.height + yMarginDropdown);
                        Vector2 dropdownSize = new Vector2(lastRect.width, heightDropdown);
                        Rect dropdownFilterRect = new Rect(dropdownPosition, dropdownSize);

                        using (new GUI.GroupScope(dropdownFilterRect, EditorStyles.helpBox))
                        {
                            Rect filterRect = new Rect(Vector2.zero, new Vector2(dropdownSize.x, 20f));

                            foreach (Filter filter in filters)
                            {
                                filter.FilterBox(filterRect, 5f);
                                filterRect.position = new Vector2(filterRect.position.x, filterRect.position.y + 20f);
                            }

                            //Vector2 marginBorder = new Vector2(5f, 5f);
                            //Rect toggleRect = new Rect(Vector2.zero + marginBorder, Vector2.one * 20f);
                            //toggle = GUI.Toggle(toggleRect, toggle, GUIContent.none);

                            //GUI.enabled = toggle;

                            //Vector2 halfSize = new Vector2((dropdownSize.x - toggleRect.width) / 2 - marginBorder.x * 2, toggleRect.height);

                            //Rect ValueRect = new Rect(toggleRect.position + new Vector2(toggleRect.width, 0f), halfSize);
                            //spells = (Spells)EditorGUI.EnumPopup(ValueRect, spells);

                            //Rect AmountRect = new Rect(ValueRect.position + new Vector2(ValueRect.width + marginBorder.x, 0f), halfSize);
                            //amountSpell = EditorGUI.IntField(AmountRect, amountSpell);
                        }

                        //GUI.enabled = true;
                    }

                    GUILayout.FlexibleSpace();
                }
                GUILayout.FlexibleSpace();

                CreateCardButton();
                if (LooseFocus())
                {
                    isCLicked = false;
                }
                GUI.enabled = !isCLicked;
            }
        }

        private void CreateCardButton()
        {
            //create card button
            GUI.contentColor = Color.green;
            if (GUILayout.Button("+", new GUIStyle(GUI.skin.button) { fontSize = 40, alignment = TextAnchor.MiddleCenter },
                GUILayout.Height(50),
                GUILayout.Width(50)
                ))
            {
                print("ouioui", MessageType.Error);
            }
            GUI.contentColor = Color.white;
        }

        private void HeaderManageCardMenu()
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