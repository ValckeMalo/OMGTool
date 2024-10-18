using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MaloProduction
{
    namespace CustomAttributes
    {
        [CustomEditor(typeof(MonoBehaviour), true)]
        public class ButtonAttributeEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();
                MonoBehaviour monoBehaviour = target as MonoBehaviour;
                var methods = monoBehaviour.GetType().GetMethods(BindingFlags.Instance |
                                                                 BindingFlags.Static |
                                                                 BindingFlags.Public |
                                                                 BindingFlags.NonPublic);

                foreach (var method in methods)
                {
                    ButtonAttribute buttonAttribute = System.Attribute.GetCustomAttribute(method, typeof(ButtonAttribute)) as ButtonAttribute;

                    if (buttonAttribute != null)
                    {
                        if (GUILayout.Button(buttonAttribute.ButtonLabel))
                        {
                            method.Invoke(monoBehaviour, null);
                        }
                    }
                }
            }
        }
    }
}