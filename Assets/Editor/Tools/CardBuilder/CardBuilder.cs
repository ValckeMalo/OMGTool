namespace MaloProduction
{
    using UnityEditor;
    using UnityEngine;

    public partial class CardBuilder : EditorWindow
    {
        #region Enum
        private enum WindowState
        {
            Home = 0,
            Modify = 1,
            Settings = 2,
        }
        private enum CardTypeFilter
        {
            All,
            Attack = CardType.Attack,
            Defense = CardType.Defense,
            Boost = CardType.Boost,
            Neutral = CardType.Neutral,
            GodPositive = CardType.Divine,
            GodNegative = CardType.Curse,
            Finisher = CardType.Finisher,
        }
        private enum Comparison
        {
            GreaterOrEqual,
            Equal,
            LessOrEqual,
        }
        private enum TargetFilter
        {
            FirstEnemy = Target.FirstEnemy,
            LastEnemy = Target.LastEnemy,
            AllEnemy = Target.AllEnemy,
            Me = Target.Me,
        }
        private enum CardNavigation
        {
            Previous,
            Next,
        }
        #endregion

        //Window
        private static CardBuilder window;

        //main var
        private WindowState state = WindowState.Home;
        private Vector2 scrollPosition;

        //Load Asset
        private CardOptions cardSettings;
        private CardLibrary cardLibrary;

        private Texture2D hoverButtonTexture;
        private Texture2D OpaqueBackgroundTexture;

        //Icon Texture
        private Texture2D TrashCanTexture;
        private Texture2D CogTexture;
        private Texture2D ForwardTexture;
        private Texture2D BackwardTexture;
        private Texture2D BackTexture;

        //directory
        static string pathCard = "Assets/ScriptableObjects/Cards/";

        [MenuItem("Tools/Card Builder")]
        public static void ShowWindow()
        {
            window = GetWindow<CardBuilder>("Card Builder");
            window.LoadAssets();
        }

        private void OnEnable() => LoadAssets();
        private void OnLostFocus() => ModifyNameCardObject();

        private void OnGUI()
        {
            switch (state)
            {
                case WindowState.Home:
                    UpdateManageCard();
                    break;
                case WindowState.Modify:
                    UpdateModifyCard();
                    break;
                case WindowState.Settings:
                    UpdateSettings();
                    break;
                default:
                    break;
            }
        }

        private void ChangeState(WindowState nextState) => state = nextState;

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

        private void LoadAssets()
        {
            cardSettings = Resources.Load("CardOptions") as CardOptions;
            hoverButtonTexture = Resources.Load("HoverButtonTexture") as Texture2D;
            OpaqueBackgroundTexture = Resources.Load("OpaqueBackground") as Texture2D;
            cardLibrary = Resources.Load("CardLibrary") as CardLibrary;

            LoadIcon();
            UpdateSerializedCard(indexCard);
            ReloadSettings();
        }

        private void LoadIcon()
        {
            TrashCanTexture = Resources.Load("Icon/TrashCan") as Texture2D;
            CogTexture = Resources.Load("Icon/Cog") as Texture2D;
            ForwardTexture = Resources.Load("Icon/Forward") as Texture2D;
            BackwardTexture = Resources.Load("Icon/Backwrad") as Texture2D;
            BackTexture = Resources.Load("Icon/Return") as Texture2D;
        }
    }
}