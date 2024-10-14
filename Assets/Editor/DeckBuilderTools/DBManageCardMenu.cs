using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MaloProduction
{
    public partial class DeckBuilderTools : EditorWindow
    {
        //Filter Box
        //////Define
        private static float Spacing = 5f;
        private static float HeightFilterBox = 100f;
        //////Filter Box Var
        private bool isFilterBoxOpen = false;
        private string nameFilter = string.Empty;
        private CardTypeFilter typeFilter = CardTypeFilter.None;
        private Filter filter = new Filter
            (new List<FilterLine>()
                {
                    new FilterLine(new List<FilterElement>()
                    {
                        new FilterElement("Wakfu Cost :"),
                        new FilterElement(Comparison.Equal),
                        new FilterElement(0,-6,6)
                    }),

                    new FilterLine(new List<FilterElement>()
                    {
                        new FilterElement("Value Card :"),
                        new FilterElement(Comparison.Equal),
                        new FilterElement(0),
                    }),

                    new FilterLine(new List<FilterElement>()
                    {
                        new FilterElement("Target :"),
                        new FilterElement(Target.FirstEnemy),
                    }),

                    new FilterLine(new List<FilterElement>()
                    {
                        new FilterElement(Spells.Poison),
                        new FilterElement(Comparison.Equal),
                        new FilterElement(0),
                    }),
                });

        private void UpdateManageCard()
        {
            HeaderManageCardMenu();
            ToolBarMangeCard();

            if (cardLibrary.cardsLibrary.Count > 0)
            {
                GridCardsButton();
            }
            else
            {
                NoneCardCreatedTitle();
            }

            GUI.enabled = true;
        }

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

                //Filter Box filter
                using (new EditorGUILayout.VerticalScope())
                {
                    GUILayout.FlexibleSpace();

                    //Filter Box Header Scope
                    using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandHeight(true)))
                    {
                        EditorGUILayout.LabelField("Other : ", GUILayout.MaxWidth(50f));

                        string label = FilterStringFormatter.GetFilterDescription(filter.ToggleCount(true));
                        if (GUILayout.Button(label,
                                             new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter, fontSize = 10, fontStyle = FontStyle.Italic },
                                             GUILayout.MaxWidth(300f)))
                        {
                            isFilterBoxOpen = isFilterBoxOpen.Invert();
                        }

                    }

                    if (isFilterBoxOpen)
                    {
                        Rect headerFilterBoxRect = GUILayoutUtility.GetLastRect();
                        Vector2 filterBoxPosition = new Vector2(headerFilterBoxRect.x, headerFilterBoxRect.y + headerFilterBoxRect.height + Spacing);
                        Vector2 filterBoxSize = new Vector2(headerFilterBoxRect.width, HeightFilterBox);
                        Rect filterBoxRect = new Rect(filterBoxPosition, filterBoxSize);

                        filter.FilterBox(filterBoxRect);
                    }

                    GUILayout.FlexibleSpace();
                }
                GUILayout.FlexibleSpace();

                AddCardButton();

                if (LooseFocus())
                {
                    isFilterBoxOpen = false;
                }

                GUI.enabled = !isFilterBoxOpen;
            }
        }

        private void AddCardButton()
        {
            //create a card button
            GUI.contentColor = Color.green;
            if (GUILayout.Button("+", new GUIStyle(GUI.skin.button) { fontSize = 40, alignment = TextAnchor.MiddleCenter },
                GUILayout.Height(50),
                GUILayout.Width(50)))
            {
                Debug.Log("ouioui");
            }
            GUI.contentColor = Color.white;
        }

        private void HeaderManageCardMenu()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
            {
                //Title
                EditorGUILayout.LabelField("Card Tool",
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

        private void GridCardsButton()
        {
            float windowWidth = EditorGUIUtility.currentViewWidth;
            float buttonWidth = 85f;
            float margin = 10f;
            float remainingWidth = windowWidth;

            GUILayout.BeginHorizontal();

            for (int i = 0; i < cardLibrary.cardsLibrary.Count; i++)
            {
                if (remainingWidth < buttonWidth + margin)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    remainingWidth = windowWidth;
                }

                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.Width(buttonWidth), GUILayout.Height(120)))
                {
                    PreviewCard(cardLibrary.cardsLibrary[i], (int)buttonWidth);

                    EditorGUILayout.LabelField(cardLibrary.cardsLibrary[i].cardName, new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleCenter }, GUILayout.Width(buttonWidth));
                }

                //draw invisible button on the top of the preview and name of the card
                Rect temp = GUILayoutUtility.GetLastRect();
                if (GUI.Button(temp, "", new GUIStyle() { hover = new GUIStyleState() { background = hoverButtonTexture } }))
                {
                    ChangeState(WindowState.ModifyCard);
                    UpdateSerializedCard(cardLibrary.cardsLibrary[i], i);
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
    }
}