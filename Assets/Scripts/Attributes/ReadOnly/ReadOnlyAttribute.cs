using System;
using UnityEngine;

namespace MVProduction
{
    namespace CustomAttributes
    {
        [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
        public class ReadOnlyAttribute : PropertyAttribute { }
    }
}