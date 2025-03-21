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
        using (new EditorGUI.PropertyScope(position, null, property))
        {
            // Calculate total height for the property field
            float propertyFieldHeight = EditorGUI.GetPropertyHeight(property, label, true);
            Rect mainRect = new Rect(position.x, position.y, position.width, propertyFieldHeight);

            if (property.managedReferenceValue == null)
            {
                DrawMenuChangeType(mainRect, property);
            }
            else
            {
                Rect buttonRect = new Rect(position.x + (position.width * 0.75f), position.y, (position.width * 0.25f), EditorGUIUtility.singleLineHeight);
                DrawMenuChangeType(buttonRect, property);

                mainRect.position += new Vector2(0f, ButtonSpacing);

                // Draw the assigned property recursively
                EditorGUI.PropertyField(mainRect, property, label, true);
            }
        }
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

    private void DrawMenuChangeType(Rect buttonRect, SerializedProperty property)
    {
        GUI.backgroundColor = Color.blue;

        if (GUI.Button(buttonRect, "Assign Type"))
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

        GUI.backgroundColor = Color.white;
    }
}