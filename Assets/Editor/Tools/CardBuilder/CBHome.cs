namespace MaloProduction
{
    using System.Collections.Generic;
    using System.Linq;

    using UnityEditor;
    using UnityEngine;

    using OMG.Tools.PreviewCard;

    public partial class CardBuilder : EditorWindow
    {
        //Filter Box
        //////Define
        private static float Spacing = 5f;
        private static float HeightFilterBox = 100f;
        //////Filter Box Var
        private bool isFilterBoxOpen = false;
        private string nameFilter = string.Empty;
        private CardTypeFilter typeFilter = CardTypeFilter.All;
        private Rect filterBoxRect;

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
                        new FilterElement(Target.FirstMonster),
                    }),

                    new FilterLine(new List<FilterElement>()
                    {
                        new FilterElement(Spells.Poison),
                        new FilterElement(Comparison.Equal),
                        new FilterElement(0),
                    }),
                });

        private CardType cardTypeCreated = CardType.Attack;

        private void UpdateManageCard()
        {
            HeaderManageCardMenu();
            FilterBar();
            UpdateBody();
            GUI.enabled = true;
            DrawFilterBox();
        }

        #region Header Bar
        private void HeaderManageCardMenu()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.MaxHeight(50f)))
            {
                //Create Card
                AddCardButton();

                //Title
                EditorGUILayout.LabelField("Card Builder",
                    new GUIStyle(EditorStyles.boldLabel) { fontSize = 20, alignment = TextAnchor.MiddleCenter },
                    GUILayout.ExpandWidth(true),
                    GUILayout.Height(50f));

                //options button
                if (GUILayout.Button(CogTexture,
                    GUILayout.Height(50f),
                    GUILayout.Width(50f)
                    ))
                {
                    ChangeState(WindowState.Settings);
                }
            }
        }

        private void AddCardButton()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.Width(175f)))
            {
                //create a card button
                GUI.contentColor = Color.green;
                if (GUILayout.Button("+", new GUIStyle(GUI.skin.button) { fontSize = 40, alignment = TextAnchor.MiddleCenter },
                    GUILayout.Height(50),
                    GUILayout.Width(50)))
                {
                    CreateACard();
                }
                GUI.contentColor = Color.white;

                using (new EditorGUILayout.VerticalScope())
                {
                    GUILayout.FlexibleSpace();
                    //type card
                    cardTypeCreated = (CardType)EditorGUILayout.EnumPopup(cardTypeCreated, GUILayout.ExpandWidth(true));
                    GUILayout.FlexibleSpace();
                }
            }
        }
        private void CreateACard()
        {
            ScriptableObject newCard = ScriptableObject.CreateInstance(typeof(CardData));
            (newCard as CardData).cardName = "NewCard";
            string newCardName = "NewCard" + ExtensionMethods.GenerateRandomString(10);
            (newCard as CardData).cardType = cardTypeCreated;
            AssetDatabase.CreateAsset(newCard, pathCard + newCardName + ".asset");
            cardLibrary.cards.Add(newCard as CardData);
        }
        #endregion

        #region Filter Bar
        private void DrawFilterBox()
        {
            if (isFilterBoxOpen)
            {
                GUI.DrawTexture(new Rect(filterBoxRect.x, filterBoxRect.y, filterBoxRect.width, filterBoxRect.height), OpaqueBackgroundTexture);
                filter.FilterBox(filterBoxRect, OpaqueBackgroundTexture);
            }
        }
        private void FilterBar()
        {
            //Filter bar scope
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
                    using (EditorGUILayout.HorizontalScope headerFilterBox = new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandHeight(true)))
                    {
                        EditorGUILayout.LabelField("Other : ", GUILayout.MaxWidth(50f));

                        string label = FilterStringFormatter.GetFilterDescription(filter.ToggleCount(true));
                        if (GUILayout.Button(label,
                                             new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter, fontSize = 10, fontStyle = FontStyle.Italic },
                                             GUILayout.MaxWidth(300f)))
                        {
                            isFilterBoxOpen = isFilterBoxOpen.Invert();
                        }

                        if (isFilterBoxOpen)
                        {
                            Rect headerFilterBoxRect = headerFilterBox.rect;
                            Vector2 filterBoxPosition = new Vector2(headerFilterBoxRect.x, headerFilterBoxRect.y + headerFilterBoxRect.height + Spacing);
                            Vector2 filterBoxSize = new Vector2(headerFilterBoxRect.width, HeightFilterBox);
                            filterBoxRect = new Rect(filterBoxPosition, filterBoxSize);
                        }
                    }

                    GUILayout.FlexibleSpace();
                }

                GUILayout.FlexibleSpace();
                ResetFilterButton();

                if (LooseFocus())
                {
                    if (isFilterBoxOpen && !filterBoxRect.Contains(Event.current.mousePosition))
                    {
                        isFilterBoxOpen = false;
                    }
                }
                GUI.enabled = !isFilterBoxOpen;
            }
        }

        private void ResetFilterButton()
        {
            if (GUILayout.Button("Reset Filter", GUILayout.MaxWidth(100f), GUILayout.ExpandHeight(true)))
            {
                ResetFilter();
            }
        }
        private void ResetFilter()
        {
            nameFilter = string.Empty;
            typeFilter = CardTypeFilter.All;

            filter = new Filter
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
                                    new FilterElement(Target.FirstMonster),
                                }),

                                new FilterLine(new List<FilterElement>()
                                {
                                    new FilterElement(Spells.Poison),
                                    new FilterElement(Comparison.Equal),
                                    new FilterElement(0),
                                }),
                            });
        }
        #endregion

        #region Body
        private void UpdateBody()
        {
            if (cardLibrary.cards.Count > 0)
            {
                List<CardData> filteredList = GetFilteredCard();

                if (filteredList.Count > 0)
                {
                    GridCardsButton(filteredList);
                }
                else
                {
                    NoCardsFound();
                }
            }
            else
            {
                NoCardsCreated();
            }
        }
        private void GridCardsButton(List<CardData> listFiltered)
        {
            float windowWidth = EditorGUIUtility.currentViewWidth;
            float buttonWidth = 85f;
            float margin = 10f;
            float remainingWidth = windowWidth;

            GUILayout.BeginHorizontal();

            for (int i = 0; i < listFiltered.Count; i++)
            {
                if (remainingWidth < buttonWidth + margin)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    remainingWidth = windowWidth;
                }

                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.Width(buttonWidth), GUILayout.Height(120)))
                {
                    PreviewCardEditor.PreviewCard(listFiltered[i], cardSettings, (int)buttonWidth);

                    EditorGUILayout.LabelField(listFiltered[i].cardName, new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleCenter }, GUILayout.Width(buttonWidth));
                }

                //draw invisible button on the top of the preview and name of the card
                Rect temp = GUILayoutUtility.GetLastRect();
                if (!isFilterBoxOpen && GUI.Button(temp, "", new GUIStyle() { hover = new GUIStyleState() { background = hoverButtonTexture } }))
                {
                    ChangeState(WindowState.Modify);
                    UpdateSerializedCard(FindIndexInLibrary(listFiltered[i]));
                }

                remainingWidth -= buttonWidth + margin;
            }

            GUILayout.EndHorizontal();
        }

        private int FindIndexInLibrary(CardData cardSearch)
        {
            int index = 0;
            foreach (CardData card in cardLibrary.cards)
            {
                if (cardSearch == card)
                {
                    return index;
                }
                index++;
            }

            Debug.LogError("Card Not Found In The Card Library");
            return 0;
        }

        private void NoCardsCreated()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true)))
            {
                EditorGUILayout.LabelField("No Cards Created", new GUIStyle(GUI.skin.label)
                { alignment = TextAnchor.MiddleCenter, fontSize = 25, fontStyle = FontStyle.Bold },
                GUILayout.ExpandHeight(true),
                GUILayout.ExpandWidth(true));
            }
        }
        private void NoCardsFound()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true)))
            {
                EditorGUILayout.LabelField("No Cards Found", new GUIStyle(GUI.skin.label)
                { alignment = TextAnchor.MiddleCenter, fontSize = 25, fontStyle = FontStyle.Bold },
                GUILayout.ExpandHeight(true),
                GUILayout.ExpandWidth(true));
            }
        }
        #endregion

        #region Filter Fonction
        private List<CardData> GetFilteredCard()
        {
            int toggleCount = filter.ToggleCount(true);
            if (nameFilter == string.Empty && typeFilter == CardTypeFilter.All && toggleCount == 0)
            {
                return cardLibrary.cards;
            }

            List<CardData> filteredList = cardLibrary.cards;

            if (nameFilter != string.Empty)
            {
                filteredList = filteredList.Where(card => card.cardName.ToLower().Replace(" ", "").Contains(nameFilter.Replace(" ", "").ToLower())).Select(card => card).ToList();
            }

            if (typeFilter != CardTypeFilter.All)
            {
                filteredList = filteredList.Where(card => card.cardType == (CardType)typeFilter).Select(card => card).ToList();
            }

            if (toggleCount > 0)
            {
                List<FilterLine> filterdLines = filter.GetLineToggle(true);

                foreach (FilterLine line in filterdLines)
                {
                    int lastIndex = line.LastIndex;
                    FilterElement lastElement = line.GetElementAt(lastIndex);
                    FilterElement penultimateElement = line.GetElementAt(lastIndex - 1);

                    switch (line.GetTypeElementAt(lastIndex))
                    {
                        case FilterElementType.Enum:
                            if (TryEnumTarget(lastElement))
                            {
                                filteredList = filteredList.Where(card => card.target == (Target)lastElement.EnumValue).Select(card => card).ToList();
                            }
                            break;

                        case FilterElementType.Int:
                            if (TryEnumComparison(penultimateElement))
                            {
                                FilterElement antepenultimateElement = line.GetElementAt(lastIndex - 2);
                                if (TryEnumSpell(antepenultimateElement))
                                {
                                    filteredList = ComparisonSpell(filteredList, (Comparison)penultimateElement.EnumValue, (Spells)antepenultimateElement.EnumValue, lastElement.IntValue);
                                }
                                else
                                {
                                    filteredList = ComparisonCardValue(filteredList, (Comparison)penultimateElement.EnumValue, lastElement.IntValue);
                                }
                            }
                            break;

                        case FilterElementType.IntSlider:
                            if (TryEnumComparison(penultimateElement))
                            {
                                filteredList = ComparisonWakfuCost(filteredList, (Comparison)penultimateElement.EnumValue, lastElement.IntValue);
                            }
                            break;

                        default:
                            break;
                    }
                }
            }


            return filteredList;
        }

        private bool TryEnumComparison(FilterElement element)
        {
            return (element.Type == FilterElementType.Enum && element.EnumValue is Comparison);
        }
        private bool TryEnumTarget(FilterElement element)
        {
            return (element.Type == FilterElementType.Enum && element.EnumValue is Target);
        }
        private bool TryEnumSpell(FilterElement element)
        {
            return (element.Type == FilterElementType.Enum && element.EnumValue is Spells);
        }

        private List<CardData> ComparisonWakfuCost(List<CardData> filteredList, Comparison comparison, int intValue)
        {
            switch (comparison)
            {
                case Comparison.GreaterOrEqual:
                    return filteredList.Where(card => card.wakfuCost >= intValue).Select(card => card).ToList();
                case Comparison.Equal:
                    return filteredList.Where(card => card.wakfuCost == intValue).Select(card => card).ToList();
                case Comparison.LessOrEqual:
                    return filteredList.Where(card => card.wakfuCost <= intValue).Select(card => card).ToList();
                default:
                    return filteredList;
            }
        }
        private List<CardData> ComparisonCardValue(List<CardData> filteredList, Comparison comparison, int intValue)
        {
            switch (comparison)
            {
                case Comparison.GreaterOrEqual:
                    return filteredList.Where(card => card.cardValue >= intValue).Select(card => card).ToList();
                case Comparison.Equal:
                    return filteredList.Where(card => card.cardValue == intValue).Select(card => card).ToList();
                case Comparison.LessOrEqual:
                    return filteredList.Where(card => card.cardValue <= intValue).Select(card => card).ToList();
                default:
                    return filteredList;
            }
        }
        private List<CardData> ComparisonSpell(List<CardData> filteredList, Comparison comparison, Spells spell, int intValue)
        {
            switch (comparison)
            {
                case Comparison.GreaterOrEqual:
                    return filteredList
                            .Where(card => card.spells.Any(spells => spells.spellType == spell && spells.amount >= intValue))
                            .ToList();
                case Comparison.Equal:
                    return filteredList
                            .Where(card => card.spells.Any(spells => spells.spellType == spell && spells.amount == intValue))
                            .ToList();
                case Comparison.LessOrEqual:
                    return filteredList
                            .Where(card => card.spells.Any(spells => spells.spellType == spell && spells.amount <= intValue))
                            .ToList();
                default:
                    return filteredList;
            }
        }
        #endregion
    }
}