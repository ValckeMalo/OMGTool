using System;
using UnityEngine;

namespace MVProduction
{
    namespace CustomAttributes
    {
        [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
        public class SpriteAttribute : PropertyAttribute
        {
            public SpriteAttribute() { }
        }
    }
}