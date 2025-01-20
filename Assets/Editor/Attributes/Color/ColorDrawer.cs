using UnityEditor;
using UnityEngine;

namespace MVProduction
{
    namespace CustomAttributes
    {
        [CustomPropertyDrawer(typeof(ColorAttribute))]
        public class ColorDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                ColorAttribute colorAttribute = attribute as ColorAttribute;

                if (colorAttribute.colorUsage == ColorUsage.All) GUI.color = colorAttribute.color;
                else if (colorAttribute.colorUsage == ColorUsage.Background) GUI.backgroundColor = colorAttribute.color;
                else if (colorAttribute.colorUsage == ColorUsage.Content) GUI.contentColor = colorAttribute.color;

                EditorGUI.PropertyField(position, property, label);

                GUI.backgroundColor = Color.white;
                GUI.color = Color.white;
                GUI.contentColor = Color.white;
            }
        }
    }
}