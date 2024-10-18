using UnityEngine;
using UnityEditor;

namespace MaloProduction
{
    namespace CustomAttributes
    {
        [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
        public class ReadOnlyDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                GUI.enabled = false; // Disable editing of the property

                EditorGUI.PropertyField(position, property, label, true);

                GUI.enabled = true; // Re-enable editing for the next properties
            }

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                return EditorGUI.GetPropertyHeight(property, label);
            }
        }
    }
}