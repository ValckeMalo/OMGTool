using System;
using UnityEngine;

namespace MaloProduction
{
    namespace CustomAttributes
    {
        [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
        public class ColorAttribute : PropertyAttribute
        {
            public readonly Color color;
            public readonly ColorUsage colorUsage;

            public ColorAttribute(float r, float g, float b, float a = 1f, bool divideBy255 = false, ColorUsage colorUsage = ColorUsage.Background)
            {
                if (divideBy255)
                {
                    r /= 255f;
                    g /= 255f;
                    b /= 255f;
                    a /= 255f;
                }

                this.color = new Color(r, g, b, a);
                this.colorUsage = colorUsage;
            }
        }

        public enum ColorUsage
        {
            All,
            Background,
            Content,
        }
    }
}