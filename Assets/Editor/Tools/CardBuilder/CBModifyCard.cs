using UnityEngine;
using UnityEditor;

namespace MaloProduction
{
    public partial class CardBuilder : EditorWindow
    {
        //SerializedObject var
        private SerializedObject currentCard;
        private SerializedProperty propCardName;
        private SerializedProperty propCardValue;
        private SerializedProperty propCardWakfu;
        private SerializedProperty propCardIcon;
        private SerializedProperty propCardType;
        private SerializedProperty propCardTarget;
        private SerializedProperty propCardSpells;

        private int indexCard;
        private bool popUpDelete = false;

        private void UpdateModifyCard()
        {
            GUI.enabled = !popUpDelete;
            HeaderModifyCard();

            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true)))
            {
                CardDataField();
                PreviewCard(currentCard.targetObject as CardData, 500);
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
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true)))
            {
                //CARD NAME
                using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true)))
                {
                    EditorGUILayout.PropertyField(propCardName);
                }

                //VALUE CARD
                using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true)))
                {
                    EditorGUILayout.PropertyField(propCardValue);
                }

                //WAKFU COST
                using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true)))
                {
                    EditorGUILayout.IntSlider(propCardWakfu, -6, 6);
                }

                //ICON CARD
                using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true)))
                {
                    EditorGUILayout.PropertyField(propCardIcon);
                }

                //CARD TYPE
                using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true)))
                {
                    EditorGUILayout.PropertyField(propCardType);
                }

                //TARGET
                using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true)))
                {
                    EditorGUILayout.PropertyField(propCardTarget);
                }

                //SPELLS LIST
                using (EditorGUILayout.ScrollViewScope scrollView = new EditorGUILayout.ScrollViewScope(scrollPosition, GUILayout.MaxHeight(250f)))
                {
                    scrollPosition = scrollView.scrollPosition;
                    using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true)))
                    {
                        EditorGUILayout.PropertyField(propCardSpells);
                    }
                }
            }

            // save
            currentCard.ApplyModifiedProperties();
            if (LooseFocus())
            {
                ModifyNameCardObject();
            }
        }

        private void ModifyNameCardObject()
        {
            if (propCardName.stringValue != "New Card" && state == WindowState.Modify)
            {
                AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(currentCard.targetObject), propCardName.stringValue);
            }
        }

        private void UpdateSerializedCard(int indexCard)
        {
            currentCard = new SerializedObject(cardLibrary.cards[indexCard]);

            propCardName = currentCard.FindProperty("cardName");
            propCardValue = currentCard.FindProperty("cardValue");
            propCardWakfu = currentCard.FindProperty("wakfuCost");
            propCardIcon = currentCard.FindProperty("iconCard");
            propCardType = currentCard.FindProperty("cardType");
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