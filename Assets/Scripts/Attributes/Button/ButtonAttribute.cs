using System;
using UnityEngine;

namespace MVProduction.CustomAttributes
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