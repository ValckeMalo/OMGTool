namespace OMG.Tools.UI
{
    using MVProduction.EditorCode;
    using OMG.Unit.HUD;
    using OMG.Unit.Status;
    using UnityEditor;
    using UnityEngine;

    public class StatusUITool : EditorWindow
    {
        private static StatusUITool window;

        private StatusUIData statusUIData = null;
        private SerializedObject soStatusUIData = null;
        private SerializedProperty propListUIData = null;

        private Vector2 scrollPos = Vector2.zero;
        private void OnEnable() => LoadAssets();

        [MenuItem("Tools/UI/Status")]
        public static void ShowWindow()
        {
            window = GetWindow<StatusUITool>("Status UI Tool");
            window.LoadAssets();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Check for unique key"))
                statusUIData.CheckUniqueKey();

            soStatusUIData.Update();

            DrawStatusUIList();

            soStatusUIData.ApplyModifiedProperties();
        }

        private void LoadAssets()
        {
            statusUIData = Resources.Load<StatusUIData>("Runtime/UI/StatusUIData");
            soStatusUIData = new SerializedObject(statusUIData);
            propListUIData = soStatusUIData.FindProperty("statusUIs");
        }

        private void DrawStatusUIList()
        {
            using (EditorGUILayout.ScrollViewScope scrollScope = new EditorGUILayout.ScrollViewScope(scrollPos))
            {
                scrollPos = scrollScope.scrollPosition;

                using (EditorGrid grid = new EditorGrid(columns: 8, style: EditorStyles.helpBox))
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
            SerializedProperty propIcon = propElement.FindPropertyRelative("icon");

            //element scope
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.Width(128f), GUILayout.Height(277f), GUILayout.MaxWidth(128f), GUILayout.MaxHeight(277f)))
            {
                propKey.enumValueIndex = (int)(StatusType)EditorGUILayout.EnumPopup((StatusType)propKey.enumValueIndex);
                propTitle.stringValue = EditorGUILayout.TextField(propTitle.stringValue);

                //the texture scope
                using (EditorGUILayout.VerticalScope imageScope = new EditorGUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.Height(128f)))
                {
                    if (propIcon != null && propIcon.objectReferenceValue != null && propIcon.objectReferenceValue is Sprite)
                        GUI.DrawTexture(GetRectTexture(new Vector2((propIcon.objectReferenceValue as Sprite).texture.width, (propIcon.objectReferenceValue as Sprite).texture.height), imageScope.rect), (propIcon.objectReferenceValue as Sprite).texture);

                    GUILayout.FlexibleSpace();//that override the space because the Gui.drawtexture work with the rect and not with the layout so i don't take place for the layout
                }

                propIcon.objectReferenceValue = EditorGUILayout.ObjectField(propIcon.objectReferenceValue, typeof(Sprite), false) as Sprite;
                propDescription.stringValue = EditorGUILayout.TextArea(propDescription.stringValue, new GUIStyle(EditorStyles.textArea) { wordWrap = true }, GUILayout.Height(60f), GUILayout.MaxWidth(128f));

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