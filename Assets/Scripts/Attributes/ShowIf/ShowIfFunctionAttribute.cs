using System;
using UnityEngine;

namespace MaloProduction
{
    namespace CustomAttributes
    {
        [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
        public class ShowIfFunctionAttribute : PropertyAttribute
        {
            public readonly string functionNameProperty;

            public ShowIfFunctionAttribute(string functionNameProperty)
            {
                this.functionNameProperty = functionNameProperty;
            }
        }
    }
}