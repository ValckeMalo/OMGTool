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

            // Récupère l'objet actuel (MonoBehaviour ou ScriptableObject)
            UnityEngine.Object targetObject = target;

            // Récupère toutes les méthodes de l'objet
            var methods = targetObject.GetType().GetMethods(BindingFlags.Instance |
                                                            BindingFlags.Static |
                                                            BindingFlags.Public |
                                                            BindingFlags.NonPublic);

            // Parcourt les méthodes et vérifie l'attribut ButtonAttribute
            foreach (var method in methods)
            {
                ButtonAttribute buttonAttribute = System.Attribute.GetCustomAttribute(method, typeof(ButtonAttribute)) as ButtonAttribute;

                if (buttonAttribute != null)
                {
                    // Si l'attribut est présent, crée un bouton avec le label spécifié ou le nom de la méthode
                    string buttonLabel = string.IsNullOrEmpty(buttonAttribute.ButtonLabel) ? method.Name : buttonAttribute.ButtonLabel;
                    if (GUILayout.Button(buttonLabel))
                    {
                        // Exécute la méthode lorsqu'on clique sur le bouton
                        method.Invoke(targetObject, null);
                    }
                }
            }
        }
    }
}
