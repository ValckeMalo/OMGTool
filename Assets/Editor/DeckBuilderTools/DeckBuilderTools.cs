using System;
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

        private void LooseFocus()
        {
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                GUI.FocusControl(null);
                Repaint();
            }
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
    }
}