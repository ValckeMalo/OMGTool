namespace MaloProduction
{
    using UnityEngine;
    using UnityEditor;

    public partial class DeckBuilderTools : EditorWindow
    {
        #region Struct
        private struct SettingsLine
        {
            public SettingsSection[] section;

            public SettingsLine(SettingsSection[] section)
            {
                if (section.Length > 2)
                {
                    Debug.LogError($"This struct SettingsSection can handle only {maxNbSection} items.\n Your array has been truncated to {maxNbSection}.");

                    // Create a new array with the maximum allowed size
                    this.section = new SettingsSection[maxNbSection];

                    // Copy the first 'maxNbSection' elements from the original array
                    for (int i = 0; i < maxNbSection; i++)
                    {
                        this.section[i] = section[i];
                    }
                }
                else
                {
                    this.section = section;
                }
            }
        }
        private struct SettingsSection
        {
            public readonly string titleSection;
            public readonly SettingsField[] field;

            public SettingsSection(string titleSection, SettingsField[] field)
            {
                this.titleSection = titleSection;
                this.field = field;
            }
        }

        private struct SettingsField
        {
            public readonly SerializedProperty property;
            public readonly string titleProperty;

            public SettingsField(SerializedProperty property, string titleProperty)
            {
                this.property = property;
                this.titleProperty = titleProperty;
            }
        }
        #endregion

        #region Define
        private const float fieldTextureHeight = 275f;
        private const float lineHeight = 300f;
        private const short nbLine = 4;
        private const float spacingWidthLine = 20f;
        private const float spacingHeightLine = 15f;
        private const short maxNbSection = 2;
        #endregion

        //VAR
        private SerializedObject settings;
        private SerializedProperty wakfuIcon;
        private SerializedProperty cardsType;

        private SettingsLine[] linesSettings = new SettingsLine[4];

        #region Reload
        private void ReloadSettings()
        {
            settings = new SerializedObject(cardSettings);

            wakfuIcon = settings.FindProperty("wakfu");
            cardsType = settings.FindProperty("cardsTypeTexture");

            ReloadSettingsLine();
            ReloadSettingsWakfu();
        }
        private void ReloadSettingsLine()
        {
            const short nbSectionCardsType = 2;
            const short nbFieldCardsType = 2;
            int indexArray = 0;

            for (int i = 0; i < linesSettings.Length; i++)
            {
                linesSettings[i] = new SettingsLine(new SettingsSection[nbSectionCardsType]);

                for (int j = 0; j < nbSectionCardsType; j++)
                {
                    if (j == 1 && i == 3)
                        return;

                    SerializedProperty fieldProp = cardsType.GetArrayElementAtIndex(indexArray);
                    SerializedProperty enumProp = fieldProp.FindPropertyRelative("type");

                    linesSettings[i].section[j] = new SettingsSection(enumProp.enumDisplayNames[enumProp.enumValueIndex], new SettingsField[nbFieldCardsType]);

                    linesSettings[i].section[j].field[0] = new SettingsField(fieldProp.FindPropertyRelative("background"), "Background");
                    linesSettings[i].section[j].field[1] = new SettingsField(fieldProp.FindPropertyRelative("iconCard"), "Background");

                    indexArray++;
                }
            }
        }
        private void ReloadSettingsWakfu()
        {
            linesSettings[3].section[1] = new SettingsSection("Wakfu", new SettingsField[1]);
            linesSettings[3].section[1].field[0] = new SettingsField(wakfuIcon, "Icon");
        }
        #endregion

        #region Main
        private void UpdateSettings()
        {
            float heightHeader = HeaderSettings();

            float maxHeightBody = position.height - heightHeader;
            SettingsBody(maxHeightBody);
        }
        #endregion

        #region Header
        private float HeaderSettings()
        {
            float heightHeader = 0f;

            using (EditorGUILayout.HorizontalScope headerScope = new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
            {
                heightHeader = headerScope.rect.height;

                //Title
                EditorGUILayout.LabelField("Settings",
                    new GUIStyle(EditorStyles.boldLabel) { fontSize = 20, alignment = TextAnchor.MiddleCenter },
                    GUILayout.ExpandWidth(true),
                    GUILayout.MinHeight(30),
                    GUILayout.MaxHeight(70),
                    GUILayout.Height(50));

                //Button Back
                if (GUILayout.Button(BackTexture,
                    GUILayout.Height(50),
                    GUILayout.Width(50)))
                {
                    ChangeState(WindowState.ManageCard);
                }
            }

            return heightHeader;
        }
        #endregion

        #region Body
        private void SettingsBody(float maxHeight)
        {
            //scroll bar scope
            using (EditorGUILayout.ScrollViewScope scrollView = new EditorGUILayout.ScrollViewScope(scrollPosition, EditorStyles.helpBox, GUILayout.ExpandWidth(true), GUILayout.MaxHeight(maxHeight)))
            {
                scrollPosition = scrollView.scrollPosition;

                for (int i = 0; i < linesSettings.Length; i++)
                {
                    Line(linesSettings[i].section);
                    GUILayout.Space(spacingHeightLine);
                }

                settings.ApplyModifiedProperties();
            }
        }


        /// <summary>
        /// This can handle only 2 section in the line
        /// </summary>
        /// <param name="settingsSections"></param>
        private void Line(SettingsSection[] settingsSections)
        {
            int nbSection = Mathf.Max(0, Mathf.Min(settingsSections.Length, maxNbSection));

            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true), GUILayout.MaxHeight(lineHeight)))
            {
                for (int i = 0; i < nbSection; i++)
                {
                    GUILayout.Space(spacingWidthLine);
                    Section(settingsSections[i]);
                }

                GUILayout.Space(spacingWidthLine);
            }
        }

        private void Section(SettingsSection section)
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField(section.titleSection, new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter });

                //fields scope
                using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true)))
                {
                    for (int i = 0; i < section.field.Length; i++)
                    {
                        FieldSection(section.field[i]);
                    }
                }
            }
        }

        private void FieldSection(SettingsField field)
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true), GUILayout.Height(fieldTextureHeight)))
            {
                //field
                EditorGUILayout.PropertyField(field.property, GUIContent.none, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

                if (field.property.objectReferenceValue != null && field.property.objectReferenceValue is Texture2D)
                {
                    Texture2D texture = (Texture2D)field.property.objectReferenceValue;
                    Rect lastRect = GUILayoutUtility.GetLastRect();
                    //ajustement pour bien combler
                    lastRect.position -= new Vector2(4f, 0f);
                    lastRect.size -= new Vector2(10f, 0f);
                    GUI.DrawTexture(lastRect, texture);
                }

                //title
                EditorGUILayout.LabelField(field.titleProperty, new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter });
            }
        }
        #endregion
    }
}