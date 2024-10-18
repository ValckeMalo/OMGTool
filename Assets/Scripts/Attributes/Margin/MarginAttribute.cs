using System;
using UnityEngine;

namespace MaloProduction
{
    namespace CustomAttributes
    {
        [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
        public class MarginAttribute : PropertyAttribute
        {
            public readonly float LeftMarginPx;
            public readonly float RightMarginPx;
            public readonly float UpMarginPx;
            public readonly float DownMarginPx;

            public MarginAttribute(float leftMarginPx, float rightMarginPx, float upMarginPx, float downMarginPx)
            {
                LeftMarginPx = leftMarginPx;
                RightMarginPx = rightMarginPx;
                UpMarginPx = upMarginPx;
                DownMarginPx = downMarginPx;
            }
        }
    }
}