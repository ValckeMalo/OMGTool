using UnityEditor;
using UnityEngine;

namespace MaloProduction
{
    namespace CustomAttributes
    {
        [CustomPropertyDrawer(typeof(SpacingAttribute))]
        public class SpacingDrawer : DecoratorDrawer
        {
            private const float marginHeight = 20f;
            private float heightSingleLine = EditorGUIUtility.singleLineHeight - 17f;

            public override void OnGUI(Rect position)
            {
                SpacingAttribute spacingAttribute = attribute as SpacingAttribute;

                Rect spacingRect = new Rect(position.x, position.y + marginHeight, position.width, heightSingleLine + spacingAttribute.WidthPx);

                EditorGUI.DrawRect(spacingRect, spacingAttribute.colorSpacing);
            }

            public override float GetHeight()
            {
                SpacingAttribute spacingAttribute = attribute as SpacingAttribute;

                //                  line                             +  margin
                return (heightSingleLine + spacingAttribute.WidthPx) + (marginHeight * 2);
            }
        }
    }
}