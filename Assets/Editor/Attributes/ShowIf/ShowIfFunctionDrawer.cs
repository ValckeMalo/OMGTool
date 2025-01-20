using UnityEditor;
using UnityEngine;
using System.Reflection;

namespace MVProduction
{
    namespace CustomAttributes
    {
        [CustomPropertyDrawer(typeof(ShowIfFunctionAttribute))]
        public class ShowIfFunctionDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                ShowIfFunctionAttribute showIfFunction = attribute as ShowIfFunctionAttribute;

                if (ShouldShowProperty(property, showIfFunction))
                {
                    EditorGUI.PropertyField(position, property, label, true);
                }
            }

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                ShowIfFunctionAttribute showIfFunction = attribute as ShowIfFunctionAttribute;

                return ShouldShowProperty(property, showIfFunction) ? EditorGUI.GetPropertyHeight(property, label, true) : 0f;
            }

            private bool ShouldShowProperty(SerializedProperty property, ShowIfFunctionAttribute showIfFunction)
            {
                MonoBehaviour target = property.serializedObject.targetObject as MonoBehaviour;

                if (target == null) return true;

                var method = target.GetType().GetMethod(showIfFunction.functionNameProperty,
                                                        BindingFlags.Instance |
                                                        BindingFlags.Public |
                                                        BindingFlags.NonPublic);

                if (method == null)
                {
                    Debug.LogWarning($"Method '{showIfFunction.functionNameProperty}' not found on {target.GetType()}");
                    return true;
                }

                if (method.ReturnType != typeof(bool) || method.GetParameters().Length != 0)
                {
                    Debug.LogWarning($"Method '{showIfFunction.functionNameProperty}' on {target.GetType()} must return bool and have no parameters");
                    return true;
                }

                return (bool)method.Invoke(target, null);
            }
        }
    }
}