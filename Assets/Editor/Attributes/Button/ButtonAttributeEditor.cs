using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MaloProduction.CustomAttributes
{
    [CustomEditor(typeof(UnityEngine.Object), true)]
    public class ButtonAttributeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            // R�cup�re l'objet actuel (MonoBehaviour ou ScriptableObject)
            UnityEngine.Object targetObject = target;

            // R�cup�re toutes les m�thodes de l'objet
            var methods = targetObject.GetType().GetMethods(BindingFlags.Instance |
                                                            BindingFlags.Static |
                                                            BindingFlags.Public |
                                                            BindingFlags.NonPublic);

            // Parcourt les m�thodes et v�rifie l'attribut ButtonAttribute
            foreach (var method in methods)
            {
                ButtonAttribute buttonAttribute = System.Attribute.GetCustomAttribute(method, typeof(ButtonAttribute)) as ButtonAttribute;

                if (buttonAttribute != null)
                {
                    // Si l'attribut est pr�sent, cr�e un bouton avec le label sp�cifi� ou le nom de la m�thode
                    string buttonLabel = string.IsNullOrEmpty(buttonAttribute.ButtonLabel) ? method.Name : buttonAttribute.ButtonLabel;
                    if (GUILayout.Button(buttonLabel))
                    {
                        // Ex�cute la m�thode lorsqu'on clique sur le bouton
                        method.Invoke(targetObject, null);
                    }
                }
            }
        }
    }
}
