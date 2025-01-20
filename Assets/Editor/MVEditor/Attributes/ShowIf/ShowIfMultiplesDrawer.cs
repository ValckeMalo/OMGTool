using UnityEditor;
using UnityEngine;

namespace MVProduction
{
    namespace CustomAttributes
    {
        [CustomPropertyDrawer(typeof(ShowIfMultiplesAttribute))]
        public class ShowIfMultiplesDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                ShowIfMultiplesAttribute ShowIfMultiples = attribute as ShowIfMultiplesAttribute;
                SerializedObject so = property.serializedObject;

                if (ShouldShow(so, ShowIfMultiples.boolProperties))
                {
                    EditorGUI.PropertyField(position, property, label, true);
                }
            }

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                ShowIfMultiplesAttribute ShowIfMultiples = attribute as ShowIfMultiplesAttribute;
                SerializedObject so = property.serializedObject;

                return ShouldShow(so, ShowIfMultiples.boolProperties) ? EditorGUI.GetPropertyHeight(property, label, true) : 0f;
            }

            private bool ShouldShow(SerializedObject so, string[] conditionName)
            {
                for (int i = 0; i < conditionName.Length; i++)
                {
                    SerializedProperty conditionProperty = so.FindProperty(conditionName[i]);

                    if (conditionProperty != null && conditionProperty.propertyType == SerializedPropertyType.Boolean)
                    {
                        if (!conditionProperty.boolValue) return false;
                        else continue;
                    }
                    else
                    {
                        Debug.LogWarning($"ShowIfMultiples : Could not find a boolean property with the name \"{conditionName[i]}\".");
                        return false;
                    }
                }

                return true;
            }
        }
    }
}