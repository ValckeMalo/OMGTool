using System;
using UnityEngine;

namespace MaloProduction
{
    namespace CustomAttributes
    {
        [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
        public class SpacingAttribute : PropertyAttribute
        {
            public readonly float WidthPx;
            public readonly Color colorSpacing;

            public SpacingAttribute(float widthPx, float r, float g, float b, float a = 1f, bool divideBy255 = false)
            {
                WidthPx = widthPx;

                if (divideBy255)
                {
                    r /= 255;
                    g /= 255;
                    b /= 255;
                    a /= 255;
                }

                colorSpacing = new Color(r, g, b, a);
            }
        }
    }
}