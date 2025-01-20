using UnityEditor;
using UnityEngine;

namespace MVProduction
{
    namespace CustomAttributes
    {
        [CustomPropertyDrawer(typeof(TitleAttribute))]
        public class TitleDrawer : DecoratorDrawer
        {
            public override float GetHeight()
            {
                return EditorGUIUtility.singleLineHeight + (int)(EditorStyles.label.fontSize * ((attribute as TitleAttribute).FontSizePercent / 100f)); ;
            }

            public override void OnGUI(Rect position)
            {
                TitleAttribute attr = (TitleAttribute)attribute;
                GUIStyle style = new GUIStyle(GUI.skin.label);
                style.alignment = TextAnchor.MiddleCenter;
                style.fontStyle = FontStyle.Bold;
                style.fontSize = (int)(EditorStyles.label.fontSize * (attr.FontSizePercent / 100f));

                EditorGUI.LabelField(position, attr.Title, style);
            }
        }
    }
}