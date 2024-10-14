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
            private string label;
            private Color contentColor;
            private Color backgroundColor;

            public PopUpButton(string label, Color contentColor, Color backgroundColor)
            {
                this.label = label;
                this.contentColor = contentColor;
                this.backgroundColor = backgroundColor;
            }

            public static PopUpButton YesButton = new PopUpButton("Ok", Color.black, Color.green);
            public static PopUpButton NoButton = new PopUpButton("Ok", Color.black, Color.red);
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
        public bool hasBackground;
        public bool disableGUI;
        public PopUpAnchor anchor;
    }

    public enum PopUpAnchor
    {
        UpperLeft,
        UpperMiddle,
        UpperRight,
        MiddleLeft,
        Middle,
        MiddleRight,
        LowerLeft,
        LowerMiddle,
        LowerRight,
    }

    public static class PopUp
    {
        public static void PopUpBox(Vector2 windowSize, Vector2 sizePopUp, PopUpContent content, PopUpSettings settings)
        {
            Vector2 anchorPosition = PopUpPosition(settings.anchor, windowSize);
            //Vector2 popUpPosition = anchorPosition
        }

        private static Vector2 PopUpPosition(PopUpAnchor anchor, Vector2 windowSize)
        {
            switch (anchor)
            {
                case PopUpAnchor.UpperLeft:
                    return Vector2.zero;

                case PopUpAnchor.UpperMiddle:
                    return new Vector2(windowSize.x / 2f, 0f);

                case PopUpAnchor.UpperRight:
                    return new Vector2(windowSize.x, 0f);

                case PopUpAnchor.MiddleLeft:
                    return new Vector2(0f, windowSize.y / 2f);

                case PopUpAnchor.Middle:
                    return new Vector2(windowSize.x / 2f, windowSize.y / 2f);

                case PopUpAnchor.MiddleRight:
                    return new Vector2(windowSize.x, windowSize.y / 2f);

                case PopUpAnchor.LowerLeft:
                    return new Vector2(0f, windowSize.y);

                case PopUpAnchor.LowerMiddle:
                    return new Vector2(windowSize.x / 2f, windowSize.y);

                case PopUpAnchor.LowerRight:
                    return new Vector2(windowSize.x, windowSize.y);

                default:
                    Debug.LogError("Anchor not recognized");
                    return Vector2.zero;
            }
        }
    }
}