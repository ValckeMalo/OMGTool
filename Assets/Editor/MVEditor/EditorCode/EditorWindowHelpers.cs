namespace MVProduction.EditorCode
{
    using OMG.Unit.Action;
    using System;
    using UnityEditor;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public static class EditorWindowHelpers
    {
        public struct DragDropData
        {
            public DragDropData(bool isDragValid, Object targetObject)
            {
                IsDragValid = isDragValid;
                TargetObject = targetObject;
            }

            public bool IsDragValid;
            public Object TargetObject;

        }

        public static DragDropData HandleDragAndDrop(Rect dropArea, Type whiteList,EditorWindow window)
        {
            Event evt = Event.current;
            DragDropData dragDropData = new DragDropData(false, null);

            switch (evt.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!dropArea.Contains(evt.mousePosition))
                    {
                        Debug.LogError("OUTSIDE");
                        dragDropData.IsDragValid = false;
                        return dragDropData;
                    }

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    dragDropData.IsDragValid = false;
                    if (DragAndDrop.objectReferences == null || DragAndDrop.objectReferences.Length <= 0)
                        Debug.LogError("NULL");
                    foreach (Object draggedObject in DragAndDrop.objectReferences)
                    {
                        //if (draggedObject.GetType().IsAssignableFrom(whiteList)) // same type by inheritance
                        if (draggedObject is EffectAction) // same type by inheritance
                        {
                            Debug.LogError("SAME");
                            dragDropData.IsDragValid = true;

                            if (evt.type == EventType.DragPerform)//if the user drop the object
                            {
                                DragAndDrop.AcceptDrag();
                                dragDropData.TargetObject = draggedObject;
                                dragDropData.IsDragValid = true;
                                window.Repaint();
                                return dragDropData;
                            }
                        }
                    }

                    return dragDropData;

                case EventType.DragExited:
                    Debug.LogError("EXITED");
                    dragDropData.IsDragValid = false;
                    return dragDropData;
            }

            Debug.LogError("NONE");
            return dragDropData;
        }

    }
}