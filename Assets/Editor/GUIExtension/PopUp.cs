namespace MaloProduction
{
    using UnityEditor;
    using UnityEngine;

    public class PopUpContent
    {
        public readonly string warning;
        public readonly PopUpButton yesButton;
        public readonly PopUpButton noButton;

        public struct PopUpButton
        {
            public readonly string Label;
            public readonly Color ContentColor;
            public readonly Color BackgroundColor;

            public PopUpButton(string label, Color contentColor, Color backgroundColor)
            {
                Label = label;
                ContentColor = contentColor;
                BackgroundColor = backgroundColor;
            }

            public static PopUpButton YesButton = new PopUpButton("Yes", Color.white, Color.green);
            public static PopUpButton NoButton = new PopUpButton("No", Color.white, Color.red);
        }

        public PopUpContent(PopUpContent popUpContent)
        {
            noButton = popUpContent.noButton;
            yesButton = popUpContent.yesButton;
            warning = popUpContent.warning;
        }

        public PopUpContent(string warning)
        {
            this.warning = warning;
            yesButton = PopUpButton.YesButton;
            yesButton = PopUpButton.NoButton;
        }

        public PopUpContent(PopUpButton yesButton, PopUpButton noButton)
        {
            warning = string.Empty;
            this.yesButton = yesButton;
            this.noButton = noButton;
        }

        public PopUpContent(string warning, PopUpButton yesButton, PopUpButton noButton)
        {
            this.warning = warning;
            this.yesButton = yesButton;
            this.noButton = noButton;
        }
    }

    public struct PopUpSettings
    {
        public Texture2D backgroundTexture;

        public bool disableGUI;
        public PopUpAnchor anchor;

        public PopUpSettings(bool disableGUI, PopUpAnchor anchor)
        {
            this.disableGUI = disableGUI;
            this.anchor = anchor;

            backgroundTexture = null;
        }
        public PopUpSettings(bool disableGUI, PopUpAnchor anchor, Texture2D backgroundTexture)
        {
            this.disableGUI = disableGUI;
            this.anchor = anchor;

            this.backgroundTexture = backgroundTexture;
        }
    }

    public enum PopUpAnchor
    {
        UpperLeft,
        UpperMiddle,
        UpperRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        LowerLeft,
        LowerMiddle,
        LowerRight,
    }
    public enum PopUpChoice
    {
        Yes,
        No,
        None,
    }

    public static class PopUp
    {
        private static PopUpContent Content;
        private static PopUpSettings Settings;

        public static PopUpChoice PopUpBox(Vector2 windowSize, Vector2 popUpSize, PopUpContent content, PopUpSettings settings)
        {
            GUI.enabled = true;

            Content = content;
            Settings = settings;

            Vector2 popUpPosition = PopUpPosition(settings.anchor, windowSize, popUpSize);
            Rect popUpRect = new Rect(popUpPosition, popUpSize);

            PopUpChoice popUpChoice;

            using (new GUI.GroupScope(popUpRect, EditorStyles.helpBox))
            {
                Background(popUpRect);
                Warning(popUpRect);
                popUpChoice = Buttons(popUpRect);
            }

            GUI.enabled = !settings.disableGUI;
            return popUpChoice;
        }

        private static Vector2 PopUpPosition(PopUpAnchor anchor, Vector2 windowSize, Vector2 popUpSize)
        {
            switch (anchor)
            {
                case PopUpAnchor.UpperLeft:
                    return Vector2.zero;

                case PopUpAnchor.UpperMiddle:
                    return new Vector2(windowSize.x / 2f - popUpSize.x / 2f, 0f);

                case PopUpAnchor.UpperRight:
                    return new Vector2(windowSize.x - popUpSize.x, 0f);

                case PopUpAnchor.MiddleLeft:
                    return new Vector2(0f, windowSize.y / 2f - popUpSize.y / 2f);

                case PopUpAnchor.MiddleCenter:
                    return new Vector2(windowSize.x / 2f - popUpSize.x / 2f, windowSize.y / 2f - popUpSize.y / 2f);

                case PopUpAnchor.MiddleRight:
                    return new Vector2(windowSize.x - popUpSize.x, windowSize.y / 2f - popUpSize.y / 2f);

                case PopUpAnchor.LowerLeft:
                    return new Vector2(0f, windowSize.y - popUpSize.y);

                case PopUpAnchor.LowerMiddle:
                    return new Vector2(windowSize.x / 2f - popUpSize.x / 2f, windowSize.y - -popUpSize.y);

                case PopUpAnchor.LowerRight:
                    return new Vector2(windowSize.x - popUpSize.x, windowSize.y - popUpSize.y);

                default:
                    Debug.LogError("Anchor not recognized");
                    return Vector2.zero;
            }
        }

        private static void Background(Rect popUpRect)
        {
            const float margin = 1f;
            Rect backgroundRect = new Rect(margin, margin, popUpRect.width - (margin * 2f), popUpRect.height - (margin * 2f));

            if (Settings.backgroundTexture != null)
            {
                GUI.DrawTexture(backgroundRect, Settings.backgroundTexture);
            }
        }

        private static void Warning(Rect popUpRect)
        {
            Vector2 warningPosition = new Vector2(0f, 0f);
            Vector2 warningSize = new Vector2(popUpRect.width, popUpRect.height * 0.8f);
            Rect warningRect = new Rect(warningPosition, warningSize);

            GUIStyle warningStyle = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
            warningStyle.fontSize = Mathf.Clamp((int)popUpRect.height / 10, 12, 50);
            GUI.Label(warningRect, Content.warning, warningStyle);
        }

        private static PopUpChoice Buttons(Rect popUpRect)
        {
            PopUpChoice popUpChoice = PopUpChoice.None;

            float margin = 5f;
            Vector2 buttonsPosition = new Vector2(margin, popUpRect.height * 0.8f);
            //size of the all area
            Vector2 buttonsSize = new Vector2(popUpRect.width - margin * 3f, popUpRect.height * 0.2f);
            //size for only one button
            Vector2 buttonSize = buttonsSize / 2f;

            Rect buttonRect = new Rect(buttonsPosition, buttonSize);

            //yes Button
            GUI.contentColor = Content.yesButton.ContentColor;
            GUI.backgroundColor = Content.yesButton.BackgroundColor;

            if (GUI.Button(buttonRect, Content.yesButton.Label))
            {
                popUpChoice = PopUpChoice.Yes;
            }

            buttonRect.position += new Vector2(buttonSize.x + margin, 0f);

            //no Button
            GUI.contentColor = Content.noButton.ContentColor;
            GUI.backgroundColor = Content.noButton.BackgroundColor;

            if (GUI.Button(buttonRect, Content.noButton.Label))
            {
                popUpChoice = PopUpChoice.No;
            }

            //reset GUI color
            GUI.contentColor = Color.white;
            GUI.backgroundColor = Color.white;

            return popUpChoice;
        }
    }
}