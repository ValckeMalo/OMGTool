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

        public static void OpenWindow(ScriptableObject obj)
        {
            window = GetWindow<ActionUITool>("Action UI Tool");
            window.targetObject = obj;
            window.Show();
        }

        private void OnGUI()
        {
            using (EditorGUILayout.VerticalScope scope = new EditorGUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true)))
            {
                if (targetObject == null)
                {
                    //EditorGUILayout.HelpBox("No ScriptableObject selected.", MessageType.Info);
                    EditorGUILayout.HelpBox("No ScriptableObject selected.", MessageType.Info);
                }
                else
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
                }

                DrawDragAndDropScope(scope.rect);
            }
        }

        private void DrawDragAndDropScope(Rect scope)
        {
            Color originalColor = GUI.backgroundColor;

            GUI.backgroundColor = isDragValid ? Color.green : Color.red;
            GUI.Box(scope, string.Empty, EditorStyles.helpBox);
            GUI.backgroundColor = originalColor;

            GUI.Label(scope, isDragValid ? "Valid ScriptableObject detected! Drop to set." : "Drag a valid ScriptableObject here.",
                      EditorStyles.whiteLabel);

            HandleDragAndDrop(scope);
        }

        private void HandleDragAndDrop(Rect dropArea)
        {
            var data = EditorWindowHelpers.DragDropArea.HandleDragAndDrop(dropArea, typeof(EffectAction), this);

            if (data == null && !data.HasValue)
            {
                return;
            }

            isDragValid = data.Value.IsDragValid;
            if (data.Value.TargetObject != null)
            {
                targetObject = data.Value.TargetObject as ScriptableObject;
                Repaint();
            }
        }
    }
}