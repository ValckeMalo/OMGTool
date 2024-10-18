using System;
using UnityEngine;

namespace MaloProduction
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