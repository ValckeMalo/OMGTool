using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using OMG.Data.Mobs.Actions;

[CustomPropertyDrawer(typeof(MobActionsList), true)]
public class MobActionsListDrawer : PropertyDrawer
{
    // Spacing for our additional button
    private const float ButtonSpacing = 2f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Calculate total height for the property field
        float propertyFieldHeight = property.managedReferenceValue == null ?
            EditorGUIUtility.singleLineHeight :
            EditorGUI.GetPropertyHeight(property, label, true);

        // We'll use a vertical layout: first the main field/assign button, then (if applicable) the "Change Type" button.
        Rect mainRect = new Rect(position.x, position.y, position.width, propertyFieldHeight);

        if (property.managedReferenceValue == null)
        {
            // Draw a button that lets you choose a concrete type
            if (GUI.Button(mainRect, "Assign Mob Action List"))
            {
                GenericMenu menu = new GenericMenu();
                // Find all non-abstract types derived from MobActionsList
                var types = AppDomain.CurrentDomain.GetAssemblies()
                            .SelectMany(asm => asm.GetTypes())
                            .Where(t => !t.IsAbstract && typeof(MobActionsList).IsAssignableFrom(t));
                foreach (var type in types)
                {
                    menu.AddItem(new GUIContent(type.Name), false, () =>
                    {
                        property.managedReferenceValue = Activator.CreateInstance(type);
                        property.serializedObject.ApplyModifiedProperties();
                    });
                }
                menu.ShowAsContext();
            }
        }
        else
        {
            // Draw the assigned property recursively
            EditorGUI.PropertyField(mainRect, property, label, true);

            // Prepare a rect for the "Change Type" button below the property field
            float buttonY = position.y + propertyFieldHeight + ButtonSpacing;
            Rect buttonRect = new Rect(position.x, buttonY, position.width, EditorGUIUtility.singleLineHeight);
            if (GUI.Button(buttonRect, "Change Mob Action List Type"))
            {
                // Reset the property to allow type re-assignment
                property.managedReferenceValue = null;
                property.serializedObject.ApplyModifiedProperties();
            }
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.managedReferenceValue == null)
        {
            return EditorGUIUtility.singleLineHeight;
        }
        else
        {
            // Height of the drawn property field plus extra height for the change button and spacing
            float height = EditorGUI.GetPropertyHeight(property, label, true);
            return height + EditorGUIUtility.singleLineHeight + ButtonSpacing;
        }
    }
}