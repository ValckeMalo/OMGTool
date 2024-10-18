using System;
using UnityEngine;

namespace MaloProduction
{
    namespace CustomAttributes
    {
        [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
        public class ReadOnlyAttribute : PropertyAttribute { }
    }
}