namespace MVProduction.EditorCode
{
    using System;
    using UnityEditor;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public static class EditorWindowHelpers
    {
        public static class DragDropArea
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

            public static DragDropData? HandleDragAndDrop(Rect dropArea, Type whiteList, EditorWindow window)
            {
                Event evt = Event.current;
                DragDropData dragDropData = new DragDropData(false, null);

                switch (evt.type)
                {
                    case EventType.DragUpdated:
                    case EventType.DragPerform:
                        if (!dropArea.Contains(evt.mousePosition))
                        {
                            dragDropData.IsDragValid = false;
                            window.Repaint();
                            return dragDropData;
                        }

                        DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                        dragDropData.IsDragValid = false;

                        foreach (Object draggedObject in DragAndDrop.objectReferences)
                        {
                            if (draggedObject != null && whiteList.IsAssignableFrom(draggedObject.GetType()))
                            {
                                dragDropData.IsDragValid = true;

                                if (evt.type == EventType.DragPerform)
                                {
                                    DragAndDrop.AcceptDrag();
                                    dragDropData.TargetObject = draggedObject;
                                    window.Repaint();
                                    return dragDropData;
                                }
                            }
                        }
                        return dragDropData;

                    case EventType.DragExited:
                        dragDropData.IsDragValid = false;
                        window.Repaint();
                        return dragDropData;
                }

                window.Repaint();
                return null;
            }
        }
    }
}