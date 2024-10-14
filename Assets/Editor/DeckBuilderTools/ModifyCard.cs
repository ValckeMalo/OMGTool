using UnityEngine;
using UnityEditor;

namespace MaloProduction
{
    public partial class DeckBuilderTools : EditorWindow
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

        private int indexCardToModify;
        private Vector2 scrollPosition;

        private void UpdateModifyCard()
        {
            HeaderModifyCard();

            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true)))
            {
                CardDataField();
                PreviewCard(currentCard.targetObject as CardData, 500);
            }

            DrawToolBar();
        }

        #region Header
        private void HeaderModifyCard()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
            {
                if (GUILayout.Button("Previous",
                    GUILayout.Height(50),
                    GUILayout.Width(50)))
                {
                    //GetPreviousCard();
                }

                EditorGUILayout.LabelField(propCardName.stringValue,
                    new GUIStyle(EditorStyles.boldLabel) { fontSize = 20, alignment = TextAnchor.MiddleCenter },
                    GUILayout.ExpandWidth(true),
                    GUILayout.MinHeight(30),
                    GUILayout.MaxHeight(70),
                    GUILayout.Height(50));

                if (GUILayout.Button("Next",
                    GUILayout.Height(50),
                    GUILayout.Width(50)))
                {
                    //GetNextCard();
                }
            }
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
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true)))
                {
                    EditorGUILayout.PropertyField(propCardSpells);
                }
                EditorGUILayout.EndScrollView();
            }
            currentCard.ApplyModifiedProperties(); // save
            LooseFocus();
        }
        #endregion

        #region Tool Bar
        private void DrawToolBar()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true)))
            {
                GUILayout.FlexibleSpace();

                GUI.backgroundColor = Color.red;
                //Delete card button
                if (GUILayout.Button("Delete",
                    GUILayout.Height(50),
                    GUILayout.Width(50)))
                {
                    //TODO add pop "Are you sure" prio important
                    DeleteCard();
                    ChangeState(WindowState.ManageCard);
                }
                GUI.backgroundColor = Color.white;

                //back to all card
                if (GUILayout.Button("Back",
                    GUILayout.Height(50),
                    GUILayout.Width(50)))
                {
                    ChangeState(WindowState.ManageCard);
                }
            }
        }

        private void DeleteCard()
        {
            CardData cardToDelete = cardLibrary.cards[indexCardToModify];
            string pathCardToDelete = AssetDatabase.GetAssetPath(cardToDelete);

            if (AssetDatabase.DeleteAsset(pathCardToDelete))
            {
                cardLibrary.cards.RemoveAt(indexCardToModify);
            }
        }
        #endregion

        private void UpdateSerializedCard(CardData cardData, int index)
        {
            currentCard = new SerializedObject(cardData);

            propCardName = currentCard.FindProperty("cardName");
            propCardValue = currentCard.FindProperty("cardValue");
            propCardWakfu = currentCard.FindProperty("wakfuCost");
            propCardIcon = currentCard.FindProperty("iconCard");
            propCardType = currentCard.FindProperty("cardType");
            propCardTarget = currentCard.FindProperty("target");
            propCardSpells = currentCard.FindProperty("spells");

            indexCardToModify = index;
        }
    }
}