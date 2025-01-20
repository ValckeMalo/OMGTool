using UnityEditor;
using UnityEngine;

namespace MVProduction
{
    namespace CustomAttributes
    {
        [CustomPropertyDrawer(typeof(MarginAttribute))]
        public class MarginDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                MarginAttribute marginAttribute = attribute as MarginAttribute;

                position.x += marginAttribute.LeftMarginPx;
                position.width -= marginAttribute.RightMarginPx;
                position.y += marginAttribute.UpMarginPx;
                position.height = marginAttribute.DownMarginPx;

                EditorGUI.PropertyField(position, property, label, true);
            }

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                MarginAttribute marginAttribute = attribute as MarginAttribute;

                return EditorGUI.GetPropertyHeight(property, label) + marginAttribute.UpMarginPx + marginAttribute.DownMarginPx;
            }
        }
    }
}