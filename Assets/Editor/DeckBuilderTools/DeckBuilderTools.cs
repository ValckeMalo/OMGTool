namespace MaloProduction
{
    using UnityEditor;
    using UnityEngine;

    public partial class DeckBuilderTools : EditorWindow
    {
        #region Enum
        private enum WindowState
        {
            ManageCard = 0,
            ModifyCard = 1,
            Settings = 2,
        }
        #endregion

        private static DeckBuilderTools window;

        private WindowState state = WindowState.ManageCard;

        private CardOptions soCardOptions;
        private Texture2D hoverButtonTexture;

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

            RefreshCardList();
        }

        private void OnEnable() => LoadAssets();

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