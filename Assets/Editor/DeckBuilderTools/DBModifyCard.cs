using UnityEngine;
using UnityEditor;

namespace MaloProduction
{
    public partial class DeckBuilderTools : EditorWindow
    {
        private SerializedObject soCard;
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
            DrawHeaderModifyCard();

            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true)))
            {
                DrawCardData();
                PreviewCard(soCard.targetObject as CardData, 500);
            }

            DrawToolBar();
        }

        private void DrawHeaderModifyCard()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
            {
                if (GUILayout.Button("Previous",
                    GUILayout.Height(50),
                    GUILayout.Width(50)))
                {
                    GetPreviousCard();
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
                    GetNextCard();
                }
            }
        }

        private void UpdateSerializedCard(CardData cardData, int index)
        {
            soCard = new SerializedObject(cardData);
            propCardName = soCard.FindProperty("cardName");
            propCardValue = soCard.FindProperty("cardValue");
            propCardWakfu = soCard.FindProperty("wakfuCost");
            propCardIcon = soCard.FindProperty("iconCard");
            propCardType = soCard.FindProperty("cardType");
            propCardTarget = soCard.FindProperty("target");
            propCardSpells = soCard.FindProperty("spells");

            indexCardToModify = index;
        }

        private void DrawCardData()
        {
            soCard.Update();
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
            soCard.ApplyModifiedProperties();
            LooseFocus();
        }

        private void DrawToolBar()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true)))
            {
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Delete",
                    GUILayout.Height(50),
                    GUILayout.Width(50)))
                {
                    DeleteCard();
                    ChangeWindow(WindowState.ManageCardMenu);
                }

                if (GUILayout.Button("Back",
                    GUILayout.Height(50),
                    GUILayout.Width(50)))
                {
                    ChangeWindow(WindowState.ManageCardMenu);
                }
            }
        }

        private void DeleteCard()
        {
            CardData cardToDelete = allCards[indexCardToModify];
            string pathCardToDelete = AssetDatabase.GetAssetPath(cardToDelete);

            if (AssetDatabase.DeleteAsset(pathCardToDelete))
            {
                print("Card delete");
            }
        }
    }
}