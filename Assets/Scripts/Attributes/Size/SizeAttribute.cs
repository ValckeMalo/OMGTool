using System;
using UnityEngine;

namespace MaloProduction
{
    namespace CustomAttributes
    {
        [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
        public class SizeAttribute : PropertyAttribute
        {
            public readonly int sizePrecent;

            public SizeAttribute(int sizePrecent)
            {
                this.sizePrecent = sizePrecent;
            }
        }
    }
}