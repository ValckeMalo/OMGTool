namespace MVProduction
{
    using UnityEngine;
    using UnityEditor;

    using OMG.Tools.PreviewCard;
    using OMG.Card.Data;

    public partial class CardBuilder : EditorWindow
    {
        private enum TargetBoostWrapper
        {
            AllCards = Target.AllCards,
            OneCard = Target.OneCard,
            RandomCard = Target.RandomCard,
        }
        private enum TargetWrapper
        {
            None = Target.None,
            Oropo = Target.Oropo,
            FirstMonster = Target.FirstMonster,
            LastMonster = Target.LastMonster,
            AllMonsters = Target.AllMonsters,
            RandomUnit = Target.RandomUnit,
        }

        //SerializedObject var
        private SerializedObject currentCard;
        private SerializedProperty propCardName;
        private SerializedProperty propCardValue;
        private SerializedProperty propCardWakfu;
        private SerializedProperty propCardIcon;
        private SerializedProperty propBackground;
        private SerializedProperty propCardType;
        private SerializedProperty propCardTarget;
        private SerializedProperty propCardSpells;
        private SerializedProperty propNeedSacrifice;
        private SerializedProperty propIsBoostable;
        private SerializedProperty propIsEtheral;

        private int indexCard;
        private bool popUpDelete = false;

        private void UpdateModifyCard()
        {
            GUI.enabled = !popUpDelete;
            HeaderModifyCard();

            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true)))
            {
                CardDataField();
                PreviewCardEditor.PreviewCard(currentCard.targetObject as CardData, cardSettings, 500f);
            }

            ModifyCardToolBar();
            PopUpDelete();
        }

        #region Header
        private void HeaderModifyCard()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
            {
                //Previous Card
                if (GUILayout.Button(BackwardTexture,
                    GUILayout.Height(50),
                    GUILayout.Width(50)))
                {
                    LooseFocus();
                    ModifyNameCardObject();
                    NavigateToCard(CardNavigation.Previous);
                }

                //Title
                EditorGUILayout.LabelField(propCardName.stringValue,
                    new GUIStyle(EditorStyles.boldLabel) { fontSize = 20, alignment = TextAnchor.MiddleCenter },
                    GUILayout.ExpandWidth(true),
                    GUILayout.MinHeight(30),
                    GUILayout.MaxHeight(70),
                    GUILayout.Height(50));

                //Next Card
                if (GUILayout.Button(ForwardTexture,
                    GUILayout.Height(50),
                    GUILayout.Width(50)))
                {
                    LooseFocus();
                    ModifyNameCardObject();
                    NavigateToCard(CardNavigation.Next);
                }
            }
        }

        private void NavigateToCard(CardNavigation direction)
        {
            int minIndex = 0;
            int maxIndex = cardLibrary.cards.Count - 1;
            if (direction == CardNavigation.Previous)
            {
                indexCard--;
                if (indexCard < 0)
                {
                    indexCard = maxIndex;
                }
            }
            else
            {
                indexCard++;
                if (indexCard > maxIndex)
                {
                    indexCard = minIndex;
                }
            }

            UpdateSerializedCard(indexCard);
        }
        #endregion

        #region Body
        private void CardDataField()
        {
            currentCard.Update();
            CardBackground cardBG = (CardBackground)propBackground.enumValueIndex;
            Target cardTarget = (Target)propCardTarget.enumValueIndex;
            CardAction cardType = (CardAction)propCardType.enumValueIndex;

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true)))
            {

                //CARD NAME
                using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true)))
                {
                    EditorGUILayout.PropertyField(propCardName);
                }

                //BACKGROUND CARD
                using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true)))
                {
                    EditorGUILayout.PropertyField(propBackground);
                }
                //TYPE CARD
                using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true)))
                {
                    GUI.enabled = false;

                    if (cardBG == CardBackground.Divine || cardBG == CardBackground.Curse || cardBG == CardBackground.Finisher)
                        GUI.enabled = true;

                    EditorGUILayout.PropertyField(propCardType);
                    GUI.enabled = true;
                }

                //NAME CARD
                using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true)))
                {
                    EditorGUILayout.PropertyField(propCardName);
                }
                //VALUE CARD
                using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true)))
                {
                    EditorGUILayout.PropertyField(propCardValue);
                }
                //VALUE CARD
                using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true)))
                {
                    EditorGUILayout.PropertyField(propCardWakfu);
                }

                //ICON CARD
                using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true)))
                {
                    EditorGUILayout.PropertyField(propCardIcon);
                }

                //BOOSTABLE CARD
                using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true)))
                {
                    if (cardBG == CardBackground.Divine || cardBG == CardBackground.Finisher)
                    {
                        propIsBoostable.boolValue = false;
                        GUI.enabled = false;
                    }

                    EditorGUILayout.PropertyField(propIsBoostable);
                    GUI.enabled = true;
                }
                //NEED SACRIFICE CARD
                using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true)))
                {
                    if (cardTarget == Target.OneCard || cardBG == CardBackground.Finisher)
                    {
                        propNeedSacrifice.boolValue = false;
                        GUI.enabled = false;
                    }

                    EditorGUILayout.PropertyField(propNeedSacrifice);
                    GUI.enabled = true;
                }
                //ETHERAL CARD
                using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true)))
                {
                    if (cardBG == CardBackground.Divine || cardBG == CardBackground.Finisher || cardBG == CardBackground.Curse)
                    {
                        propIsEtheral.boolValue = false;
                        GUI.enabled = false;
                    }

                    EditorGUILayout.PropertyField(propIsEtheral);
                    GUI.enabled = true;
                }

                //TARGET CARD
                using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true)))
                {
                    if (cardType == CardAction.Boost)
                    {
                        propCardTarget.enumValueIndex = (int)((CardAction)EditorGUILayout.EnumPopup("Target", (TargetBoostWrapper)propCardTarget.enumValueIndex));
                    }
                    else if (cardType == CardAction.Defense || cardType == CardAction.Neutral)
                    {
                        GUI.enabled = false;
                        propCardTarget.enumValueIndex = (int)Target.Oropo;
                        EditorGUILayout.PropertyField(propCardTarget);
                        GUI.enabled = true;
                    }
                    else
                    {
                        propCardTarget.enumValueIndex = (int)((CardAction)EditorGUILayout.EnumPopup("Target", (TargetWrapper)propCardTarget.enumValueIndex));
                    }
                }

                //SPELLS CARD
                using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true)))
                {
                    EditorGUILayout.PropertyField(propCardSpells);
                }
            }

            // save
            CardBackground newCardBG = (CardBackground)propBackground.enumValueIndex;
            if (cardBG != newCardBG)
            {
                if (newCardBG == CardBackground.Attack)
                    propCardType.enumValueIndex = (int)CardAction.Attack;
                else if (newCardBG == CardBackground.Defense)
                    propCardType.enumValueIndex = (int)CardAction.Defense;
                else if (newCardBG == CardBackground.Boost)
                    propCardType.enumValueIndex = (int)CardAction.Boost;
                else if (newCardBG == CardBackground.Neutral)
                    propCardType.enumValueIndex = (int)CardAction.Neutral;
            }

            currentCard.ApplyModifiedProperties();

            if (LooseFocus())
            {
                ModifyNameCardObject();
            }
        }

        private void ModifyNameCardObject()
        {
            if (state == WindowState.Modify && propCardName != null && propCardName.stringValue != "New Card")
            {
                AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(currentCard.targetObject), propCardName.stringValue);
            }
        }

        private void UpdateSerializedCard(int indexCard)
        {
            currentCard = new SerializedObject(cardLibrary.cards[indexCard]);

            propBackground = currentCard.FindProperty("background");
            propCardType = currentCard.FindProperty("type");

            propCardValue = currentCard.FindProperty("value");
            propCardWakfu = currentCard.FindProperty("wakfu");
            propCardName = currentCard.FindProperty("name");

            propCardIcon = currentCard.FindProperty("icon");

            propIsBoostable = currentCard.FindProperty("isBoostable");
            propNeedSacrifice = currentCard.FindProperty("needSacrifice");
            propIsEtheral = currentCard.FindProperty("isEtheral");

            propCardTarget = currentCard.FindProperty("target");

            propCardSpells = currentCard.FindProperty("spells");

            this.indexCard = indexCard;
        }
        #endregion

        #region Tool Bar
        private void ModifyCardToolBar()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true)))
            {
                GUILayout.FlexibleSpace();

                GUI.backgroundColor = Color.red;
                //Delete card button
                if (GUILayout.Button(TrashCanTexture,
                    GUILayout.Height(50),
                    GUILayout.Width(50)))
                {
                    popUpDelete = true;
                }
                GUI.backgroundColor = Color.white;

                //back to all card
                if (GUILayout.Button(BackTexture,
                    GUILayout.Height(50),
                    GUILayout.Width(50)))
                {
                    ChangeState(WindowState.Home);
                }
            }
        }

        private void PopUpDelete()
        {
            if (popUpDelete)
            {
                GUI.enabled = true;
                PopUpChoice popUpChoice = PopUp.PopUpBox(position.size, position.size * 0.5f,
                                                         new PopUpContent("Are You Sure To Delete This Card",
                                                                          PopUpContent.PopUpButton.YesButton,
                                                                          PopUpContent.PopUpButton.NoButton),
                                                         new PopUpSettings(true, PopUpAnchor.MiddleCenter, OpaqueBackgroundTexture));

                PopUpChoiceAction(popUpChoice);
            }
        }
        private void PopUpChoiceAction(PopUpChoice popUpChoice)
        {
            switch (popUpChoice)
            {
                case PopUpChoice.Yes:
                    popUpDelete = false;
                    DeleteCard();
                    ChangeState(WindowState.Home);
                    GUI.enabled = true;
                    break;
                case PopUpChoice.No:
                    popUpDelete = false;
                    GUI.enabled = true;
                    break;
                case PopUpChoice.None:
                    GUI.enabled = false;
                    break;
                default:
                    Debug.Log("Don't Recognized The Value");
                    GUI.enabled = false;
                    break;
            }
        }
        private void DeleteCard()
        {
            CardData cardToDelete = cardLibrary.cards[indexCard];
            string pathCardToDelete = AssetDatabase.GetAssetPath(cardToDelete);

            if (AssetDatabase.DeleteAsset(pathCardToDelete))
            {
                cardLibrary.cards.RemoveAt(indexCard);
            }
        }
        #endregion
    }
}