using System;
using UnityEngine;

namespace MaloProduction
{
    namespace CustomAttributes
    {
        [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
        public class TitleAttribute : PropertyAttribute
        {
            public readonly string Title;
            public readonly TextAnchor Anchor;
            public readonly FontStyle Style;
            public readonly int FontSizePercent;

            public TitleAttribute(string title, int sizePercent = 150, FontStyle fontStyle = FontStyle.Bold, TextAnchor anchor = TextAnchor.MiddleCenter)
            {
                Title = title;
                Anchor = anchor;
                Style = fontStyle;
                FontSizePercent = sizePercent;
            }
        }
    }
}