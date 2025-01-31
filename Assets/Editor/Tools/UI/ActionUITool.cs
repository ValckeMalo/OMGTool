namespace OMG.Tools.UI
{
    using MVProduction.EditorCode;
    using OMG.Unit.Action;
    using UnityEditor;
    using UnityEngine;

    public class ActionUITool : EditorWindow
    {
        private ScriptableObject targetObject;
        private bool isDragValid = false; // Track if the dragged object is valid
        private static ActionUITool window;

        [MenuItem("Tools/UI/Action")]
        public static void OpenWindow()
        {
            window = GetWindow<ActionUITool>("Action UI Tool");
            window.targetObject = null;
            window.Show();
        }

        public static void OpenWindow(UnitAction obj = null)
        {
            window = GetWindow<ActionUITool>("Action UI Tool");
            window.targetObject = obj;
            window.Show();
        }

        private void OnGUI()
        {
            using (EditorGUILayout.VerticalScope scope = new EditorGUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true)))
            {
                DrawDragAndDropScope(scope.rect);

                if (targetObject != null)
                {
                    EditorGUILayout.LabelField($"Viewing: {targetObject.name}", EditorStyles.boldLabel);
                    EditorGUILayout.Space();

                    SerializedObject serializedObject = new SerializedObject(targetObject);
                    SerializedProperty property = serializedObject.GetIterator();

                    property.NextVisible(true);
                    while (property.NextVisible(false))
                    {
                        EditorGUILayout.PropertyField(property, true);
                    }

                    serializedObject.ApplyModifiedProperties();

                    //Preview
                    GUILayout.FlexibleSpace();//push it at the bottom
                    using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.Height(160f), GUILayout.ExpandWidth(true)))
                    {
                        GUILayout.FlexibleSpace();//center them
                        using (EditorGUILayout.VerticalScope iconScope = new EditorGUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.ExpandHeight(true), GUILayout.Width(160f)))
                        {
                            SerializedProperty propUnitActionUI = serializedObject.FindProperty("unitActionUI");
                            if (propUnitActionUI == null || propUnitActionUI.boxedValue == null) return;
                            SerializedProperty propIcon = propUnitActionUI.FindPropertyRelative("icon");
                            if (propIcon == null || propIcon.objectReferenceValue == null && propIcon.objectReferenceValue is not Sprite) return;

                            Texture textureIcon = (propIcon.objectReferenceValue as Sprite).texture;

                            GUI.DrawTexture(GetRectTexture(GetTextureSize(textureIcon), iconScope.rect), textureIcon);
                            GUILayout.FlexibleSpace();
                        }
                        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.ExpandHeight(true), GUILayout.Width(160f)))
                        {
                            EditorGUILayout.LabelField(new SerializedObject(targetObject).FindProperty("Value").intValue.ToString(), new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleLeft, fontSize = 50 }, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
                        }
                        GUILayout.FlexibleSpace();//center them
                    }
                }
            }

            LooseFocus();
        }

        //TODO THROW THEM IN EditorWindowHelper
        private Vector2 GetTextureSize(Texture texture)
        {
            if (texture == null) return Vector2.zero;

            return new Vector2(texture.width, texture.height);
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


        private void DrawDragAndDropScope(Rect scope)
        {
            Color originalColor = GUI.backgroundColor;

            GUI.backgroundColor = targetObject != null ? Color.white : isDragValid ? Color.green : Color.red;
            GUI.Box(scope, string.Empty, EditorStyles.helpBox);
            GUI.backgroundColor = originalColor;

            HandleDragAndDrop(scope);
        }
        private void HandleDragAndDrop(Rect dropArea)
        {
            var data = EditorWindowHelpers.DragDropArea.HandleDragAndDrop(dropArea, typeof(UnitAction), this);

            if (data != null && data.HasValue)
            {
                isDragValid = data.Value.IsDragValid;
                if (data.Value.TargetObject != null)
                {
                    targetObject = data.Value.TargetObject as ScriptableObject;
                    Repaint();
                }
            }
        }
    }
}