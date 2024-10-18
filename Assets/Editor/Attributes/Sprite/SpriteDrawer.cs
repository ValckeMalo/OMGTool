using System.Globalization;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace MaloProduction
{
    namespace CustomAttributes
    {
        [CustomPropertyDrawer(typeof(SpriteAttribute))]
        public class SpriteDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                if (property.propertyType != SerializedPropertyType.ObjectReference && property.objectReferenceValue is not Sprite)
                {
                    Debug.LogWarning($"SpriteAttribute : '{property.name}' is not a Sprite type.");
                    return;
                }

                property.objectReferenceValue = EditorGUILayout.ObjectField(property.name.ToDisplayName(), property.objectReferenceValue, typeof(Sprite), false) as Sprite;
            }

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                if (property.propertyType != SerializedPropertyType.ObjectReference && property.objectReferenceValue is not Sprite)
                {
                    Debug.LogWarning($"SpriteAttribute : '{property.name} is not a Sprite type.");
                    return 0f;
                }

                return 0f;
            }
        }

        public static class TypeExtensions
        {
            public static string ToDisplayName(this string input)
            {
                if (string.IsNullOrEmpty(input))
                {
                    return input;
                }

                StringBuilder result = new StringBuilder();
                result.Append(char.ToUpper(input[0], CultureInfo.InvariantCulture));

                for (int i = 1; i < input.Length; i++)
                {
                    if (char.IsUpper(input[i]))
                    {
                        result.Append(' ');
                    }
                    result.Append(input[i]);
                }

                return result.ToString();
            }
        }
    }
}