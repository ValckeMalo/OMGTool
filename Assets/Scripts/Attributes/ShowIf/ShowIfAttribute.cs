using UnityEngine;

namespace MVProduction
{
    namespace CustomAttributes
    {
        [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
        public class ShowIfAttribute : PropertyAttribute
        {
            public string boolProperty { get; private set; }

            public ShowIfAttribute(string propertyCondition)
            {
                boolProperty = propertyCondition;
            }
        }
    }
}