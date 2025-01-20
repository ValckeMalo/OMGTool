using UnityEditor;
using UnityEngine;

namespace MVProduction
{
    namespace CustomAttributes
    {
        [CustomPropertyDrawer(typeof(ShowIfAttribute))]
        public class ShowIfDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                ShowIfAttribute showIf = attribute as ShowIfAttribute;
                SerializedObject so = property.serializedObject;

                if (ShouldShow(so, showIf.boolProperty))
                {
                    EditorGUI.PropertyField(position, property, label, true);
                }
            }

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                ShowIfAttribute showIf = attribute as ShowIfAttribute;
                SerializedObject so = property.serializedObject;

                return ShouldShow(so, showIf.boolProperty) ? EditorGUI.GetPropertyHeight(property, label, true) : 0f;
            }

            private bool ShouldShow(SerializedObject so, string consitionName)
            {
                SerializedProperty conditionProperty = so.FindProperty(consitionName);

                if (conditionProperty != null && conditionProperty.propertyType == SerializedPropertyType.Boolean)
                {
                    return conditionProperty.boolValue;
                }
                else
                {
                    Debug.LogWarning($"ShowIf : Could not find a boolean property with the name \"{consitionName}\".");
                    return false;
                }
            }
        }
    }
}