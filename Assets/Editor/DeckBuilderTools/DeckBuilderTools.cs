using System.IO;
using UnityEditor;
using UnityEngine;

namespace MaloProduction
{
    public partial class DeckBuilderTools : EditorWindow
    {
        private enum WindowState
        {
            MainMenu,
            ManageCardMenu,
            CreateCard,
            ModifyCard,
            OptionMenu,
        }

        private WindowState currentWindow = WindowState.MainMenu;
        private string cardPath = Application.dataPath + "/ScriptableObjects/Cards";
        private string SOPath = Application.dataPath + "/ScriptableObjects";
        private CardOptions soCardOptions;
        private static DeckBuilderTools window;

        [MenuItem("Tools/Deck Builder")]
        public static void ShowWindow()
        {
            window = GetWindow<DeckBuilderTools>("Deck Builder");
            window.LoadAssets();
        }

        private void LoadAssets()
        {
            soCardOptions = Resources.Load("CardOptions") as CardOptions;
        }

        private void ChangeWindow(WindowState nextWindow)
        {
            currentWindow = nextWindow;

            if (currentWindow == WindowState.ManageCardMenu)
            {
                InitManageCardMenu();
            }
        }

        public void UpdateWindow()
        {
            switch (currentWindow)
            {
                case WindowState.MainMenu:
                    UpdateMainMenu();
                    break;
                case WindowState.ManageCardMenu:
                    UpdateManagerCardMenu();
                    break;
                case WindowState.CreateCard:
                    break;
                case WindowState.ModifyCard:
                    UpdateModifyCard();
                    break;
                case WindowState.OptionMenu:
                    UpdateOptionsMenu();
                    break;

                default:
                    break;
            }
        }

        private void OnGUI()
        {
            UpdateWindow();
        }

        private bool CheckFileExist(string path)
        {
            return Directory.Exists(path);
        }

        //create the file 
        private void CreateFileNonexistent()
        {
            if (!CheckFileExist(SOPath) && !CheckFileExist(cardPath))
            {
                Directory.CreateDirectory(cardPath);
            }
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

        private enum MessageType
        {
            Message,
            Warning,
            Error,
        }
    }
}