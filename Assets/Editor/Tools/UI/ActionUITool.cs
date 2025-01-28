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
                }
                else
                {
                    //EditorGUILayout.LabelField($"Viewing: {targetObject.name}", EditorStyles.boldLabel);
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
#if false
            Color originalColor = GUI.backgroundColor;
            Repaint();
            EditorWindowHelpers.DragDropData data = EditorWindowHelpers.HandleDragAndDrop(scope, typeof(UnitAction), window);
            

            if (data.TargetObject != null)
                targetObject = data.TargetObject as UnitAction;

            GUI.backgroundColor = data.IsDragValid ? Color.green : Color.red;
            Repaint();
            Debug.LogWarning(GUI.backgroundColor);
            Debug.Log(data.IsDragValid);
            GUI.Box(scope, string.Empty, EditorStyles.helpBox);
            GUI.backgroundColor = originalColor;

            GUI.Label(scope, data.IsDragValid ? "Valid ScriptableObject detected! Drop to set." : "Drag a valid ScriptableObject here.", EditorStyles.whiteLabel);
#else
            Color originalColor = GUI.backgroundColor;

            // Change the background color based on validity
            GUI.backgroundColor = isDragValid ? Color.green : Color.red;
            GUI.Box(scope, string.Empty, EditorStyles.helpBox);
            GUI.backgroundColor = originalColor;

            // Add help text inside the scope
            GUI.Label(scope, isDragValid ? "Valid ScriptableObject detected! Drop to set." : "Drag a valid ScriptableObject here.",
                      EditorStyles.whiteLabel);

            // Handle drag-and-drop logic
            HandleDragAndDrop(scope);
#endif
        }

        private void HandleDragAndDrop(Rect dropArea)
        {
            Event evt = Event.current;

            switch (evt.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!dropArea.Contains(evt.mousePosition))
                    {
                        isDragValid = false;
                        return;
                    }

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    isDragValid = false;

                    foreach (Object draggedObject in DragAndDrop.objectReferences)
                    {
                        if (draggedObject is UnitAction)
                        {
                            isDragValid = true;

                            if (evt.type == EventType.DragPerform)
                            {
                                DragAndDrop.AcceptDrag();
                                targetObject = draggedObject as ScriptableObject;
                                Repaint();
                                break;
                            }
                        }
                    }
                    break;

                case EventType.DragExited:
                    isDragValid = false;
                    break;
            }
        }
    }
}