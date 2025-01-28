namespace OMG.Tools.UI
{
    using MVProduction.EditorCode;
    using OMG.Card.UI;
    using OMG.Unit.Status;
    using UnityEditor;
    using UnityEngine;

    public class CardUITool : EditorWindow
    {
        private static CardUITool window;

        private CardUIData cardUIData = null;

        private SerializedObject soCardUIData = null;
        private SerializedProperty propListUIData = null;
        private SerializedProperty propInitiativeDesc = null;
        private SerializedProperty propNeedSacrificeDesc = null;
        private SerializedProperty propEtheralDesc = null;

        private Vector2 scrollPos = Vector2.zero;
        private void OnEnable() => LoadAssets();

        [MenuItem("Tools/UI/Card")]
        public static void ShowWindow()
        {
            window = GetWindow<CardUITool>("Card UI Tool");
            window.LoadAssets();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Check for unique key"))
                cardUIData.CheckUniqueKey();

            soCardUIData.Update();

            DrawMainDescription();

            DrawCardUIList();

            soCardUIData.ApplyModifiedProperties();
        }

        private void LoadAssets()
        {
            cardUIData = Resources.Load<CardUIData>("Runtime/UI/CardUIData");
            soCardUIData = new SerializedObject(cardUIData);
            propListUIData = soCardUIData.FindProperty("cardUIData");
            propInitiativeDesc = soCardUIData.FindProperty("initiativeDescription");
            propNeedSacrificeDesc = soCardUIData.FindProperty("needSacrificeDescription");
            propEtheralDesc = soCardUIData.FindProperty("etheralDescription");
        }

        private void DrawMainDescription()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.PropertyField(propInitiativeDesc);
                EditorGUILayout.PropertyField(propNeedSacrificeDesc);
                EditorGUILayout.PropertyField(propEtheralDesc);
            }
        }

        private void DrawCardUIList()
        {
            using (EditorGUILayout.ScrollViewScope scrollScope = new EditorGUILayout.ScrollViewScope(scrollPos))
            {
                scrollPos = scrollScope.scrollPosition;

                using (EditorGrid grid = new EditorGrid(columns: 6, style: EditorStyles.helpBox))
                {
                    for (int i = 0; i < propListUIData.arraySize; i++)
                    {
                        grid.AddCell(() =>
                        {
                            if (DrawElement(propListUIData.GetArrayElementAtIndex(i)))
                            {
                                propListUIData.DeleteArrayElementAtIndex(i);
                                i--;
                            }
                        });
                    }

                    grid.AddCell(DrawAddElement);
                }
            }

            LooseFocus();
        }

        private void DrawAddElement()
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.Width(128f), GUILayout.Height(277f)))
            {
                GUI.backgroundColor = Color.green;
                if (GUILayout.Button("+", GUILayout.ExpandHeight(true)))
                {
                    propListUIData.InsertArrayElementAtIndex(propListUIData.arraySize);
                }
                GUI.backgroundColor = Color.white;
            }
        }


        private bool DrawElement(SerializedProperty propElement)
        {
            SerializedProperty propKey = propElement.FindPropertyRelative("key");
            SerializedProperty propTitle = propElement.FindPropertyRelative("title");
            SerializedProperty propDescription = propElement.FindPropertyRelative("description");

            //element scope
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.Width(128f), GUILayout.Height(277f), GUILayout.MaxWidth(192f), GUILayout.MaxHeight(277f)))
            {
                propKey.enumValueIndex = (int)(StatusType)EditorGUILayout.EnumPopup((StatusType)propKey.enumValueIndex);
                propTitle.stringValue = EditorGUILayout.TextField(propTitle.stringValue);
                propDescription.stringValue = EditorGUILayout.TextArea(propDescription.stringValue, new GUIStyle(EditorStyles.textArea) { wordWrap = true }, GUILayout.ExpandHeight(true), GUILayout.MaxWidth(192f));

                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("-"))
                {
                    GUI.backgroundColor = Color.white;
                    return true;
                }
                GUI.backgroundColor = Color.white;
                return false;
            }
        }

        private Rect GetRectTexture(Vector2 textureSize, Rect targetRect)
        {
            float textureAspect = textureSize.x / textureSize.y;
            float rectAspect = targetRect.width / targetRect.height;

            if (textureAspect > rectAspect)
            {
                float height = targetRect.width / textureAspect;
                targetRect = new Rect(targetRect.x, targetRect.y + (targetRect.height - height) / 2, targetRect.width, height);
            }
            else
            {
                float width = targetRect.height * textureAspect;
                targetRect = new Rect(targetRect.x + (targetRect.width - width) / 2, targetRect.y, width, targetRect.height);
            }

            return targetRect;
        }

        private bool LooseFocus()
        {
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                GUI.FocusControl(null);
                Repaint();
                return true;
            }
            return false;
        }
    }
}