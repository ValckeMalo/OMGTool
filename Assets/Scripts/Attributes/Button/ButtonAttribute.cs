using System;
using UnityEngine;

namespace MaloProduction
{
    namespace CustomAttributes
    {
        [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
        public class ButtonAttribute : PropertyAttribute
        {
            public string ButtonLabel { get; private set; }

            public ButtonAttribute(string buttonLabel)
            {
                ButtonLabel = buttonLabel;
            }
        }
    }
}