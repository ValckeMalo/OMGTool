using System;
using UnityEngine;

namespace MaloProduction
{
    namespace CustomAttributes
    {
        [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
        public class ShowIfMultiplesAttribute : PropertyAttribute
        {
            public readonly string[] boolProperties;

            public ShowIfMultiplesAttribute(params string[] boolProperties)
            {
                this.boolProperties = boolProperties;
            }
        }
    }
}