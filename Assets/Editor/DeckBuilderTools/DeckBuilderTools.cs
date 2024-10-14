using System.IO;
using UnityEditor;
using UnityEngine;

namespace MaloProduction
{
    public partial class DeckBuilderTools : EditorWindow
    {
        #region Enum
        private enum MessageType
        {
            Message,
            Warning,
            Error,
        }
        private enum WindowState
        {
            ManageCard = 0,
            ModifyCard = 1,
            Settings = 2,
        }
        #endregion

        private static DeckBuilderTools window;

        private WindowState state = WindowState.ManageCard;

        private string cardPath = Application.dataPath + "/ScriptableObjects/Cards";
        private string SOPath = Application.dataPath + "/ScriptableObjects";

        private CardOptions soCardOptions;

        private Texture2D hoverButtonTexture;
        private GUIStyle transparentButton;

        [MenuItem("Tools/Deck Builder")]
        public static void ShowWindow()
        {
            window = GetWindow<DeckBuilderTools>("Deck Builder");
            window.LoadAssets();
        }

        private void LoadAssets()
        {
            soCardOptions = Resources.Load("CardOptions") as CardOptions;
            hoverButtonTexture = Resources.Load("HoverButtonTexture") as Texture2D;

            transparentButton = new GUIStyle() { hover = new GUIStyleState() { background = hoverButtonTexture } };

            RefreshCardList();
        }

        private void OnGUI()
        {
            switch (state)
            {
                case WindowState.ManageCard:
                    UpdateManageCard();
                    break;
                case WindowState.ModifyCard:
                    UpdateModifyCard();
                    break;
                case WindowState.Settings:
                    UpdateSettings();
                    break;
                default:
                    break;
            }
        }

        private void ChangeState(WindowState nextState)
        {
            state = nextState;

            if (state == WindowState.ManageCard)
            {
                RefreshCardList();
            }
        }

        private bool IsFileExist(string path)
        {
            return Directory.Exists(path);
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

        private enum ClickEvent
        {
            InsideBox,
            OutsideBox,
        }

        private bool LooseFocus(ClickEvent clickEvent, Rect dimensionRect)
        {
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                GUI.FocusControl(null);
                Repaint();

                if (clickEvent == ClickEvent.OutsideBox && !IsPointInsideBox(Event.current.mousePosition, dimensionRect))
                {
                    print(true);
                    return true;
                }
            }
            return false;
        }

        private bool IsPointInsideBox(Vector2 pointPosition, Rect box)
        {
            return box.Contains(pointPosition);
        }

        private void print(string message, MessageType messageType = MessageType.Message)
        {
            switch (messageType)
            {
                case MessageType.Message:
                    Debug.Log(message);
                    break;
                case MessageType.Warning:
                    Debug.LogWarning(message);
                    break;
                case MessageType.Error:
                    Debug.LogError(message);
                    break;
                default:
                    break;
            }
        }

        private void print(bool @bool, MessageType messageType = MessageType.Message)
        {
            string message = @bool.ToString();
            switch (messageType)
            {
                case MessageType.Message:
                    Debug.Log(message);
                    break;
                case MessageType.Warning:
                    Debug.LogWarning(message);
                    break;
                case MessageType.Error:
                    Debug.LogError(message);
                    break;
                default:
                    break;
            }
        }

        private void print(int @int, MessageType messageType = MessageType.Message)
        {
            string message = @int.ToString();
            switch (messageType)
            {
                case MessageType.Message:
                    Debug.Log(message);
                    break;
                case MessageType.Warning:
                    Debug.LogWarning(message);
                    break;
                case MessageType.Error:
                    Debug.LogError(message);
                    break;
                default:
                    break;
            }
        }

        private void print(Rect rect, MessageType messageType = MessageType.Message)
        {
            string message = rect.ToString();
            switch (messageType)
            {
                case MessageType.Message:
                    Debug.Log(message);
                    break;
                case MessageType.Warning:
                    Debug.LogWarning(message);
                    break;
                case MessageType.Error:
                    Debug.LogError(message);
                    break;
                default:
                    break;
            }
        }
    }
}