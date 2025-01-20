using UnityEditor;
using UnityEngine;

namespace MVProduction
{
    namespace CustomAttributes
    {
        [CustomPropertyDrawer(typeof(SizeAttribute))]
        public class SizeDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                if (property.propertyType != SerializedPropertyType.String)
                {
                    Debug.LogWarning($"Size : '{property.name}' is not a string.");
                    return;
                }

                int originalFontSize = EditorStyles.label.fontSize;
                float mulipierFontSize = (attribute as SizeAttribute).sizePrecent / 100f;
                int newFontSize = (int)(EditorStyles.label.fontSize * mulipierFontSize);
                EditorStyles.label.fontSize = newFontSize;

                EditorGUI.PropertyField(position, property, label, true);

                EditorStyles.label.fontSize = originalFontSize;
            }

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                return EditorGUI.GetPropertyHeight(property, label);
            }
        }
    }
}