namespace OMG.Tools.MobCreator
{
    using OMG.Data.Mobs;
    using OMG.Data.Mobs.Behaviour;
    using OMG.Data.Utils;
    using OMG.Game.Fight.Entities;
    using System.Collections.Generic;
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    public class MobCreatorWindow : EditorWindow
    {
        private string mobName;
        private GameObject mobPrefab;
        private Vector2 scrollPos = Vector2.zero;

        private float WindowWidth => position.width;

        private WindowState state = WindowState.Main;
        private string nameFilter = string.Empty;

        private MobData originalMob = null;
        private MobData copyMob = null;
        SerializedObject SOCopyMob;
        private string selectedGUID = string.Empty;

        private enum WindowState
        {
            Main,
            Search,
            Modify,
            Create,
        }

        [MenuItem("OMG Tools/Mob Creator")]
        public static void ShowWindow()
        {
            GetWindow<MobCreatorWindow>("Mob Creator");
        }

        private void OnGUI()
        {
            switch (state)
            {
                case WindowState.Main:
                    Main();
                    break;

                case WindowState.Create:
                    Create();
                    break;

                case WindowState.Search:
                    Search();
                    break;

                case WindowState.Modify:
                    Modify();
                    break;

                default:
                    break;
            }

            LooseFocus();
        }

        private void Main()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true)))
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("Mob", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
            }

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true)))
            {
                GUILayout.FlexibleSpace();
                using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true)))
                {
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Search", GUILayout.Height(200f), GUILayout.Width(200f)))
                    {
                        state = WindowState.Search;
                    }
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Create", GUILayout.Height(200f), GUILayout.Width(200f)))
                    {
                        state = WindowState.Create;
                    }
                    GUILayout.FlexibleSpace();
                }
                GUILayout.FlexibleSpace();
            }
        }

        private void Search()
        {
            //title
            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true)))
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("Search", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Exit"))
                {
                    state = WindowState.Main;
                }
                GUI.backgroundColor = Color.white;
            }

            //filter	
            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true)))
            {
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField("Name : ", GUILayout.MaxWidth(50f));
                nameFilter = EditorGUILayout.TextField(nameFilter, GUILayout.MaxWidth(150f));
                GUILayout.FlexibleSpace();
            }

            //grid mob so
            string[] mobDatasGUID = AssetDatabase.FindAssets("t:MobData", null);

            if (mobDatasGUID != null && mobDatasGUID.Length > 0)
            {
                using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true)))
                {
                    MobData mobData = null;
                    foreach (string guid in mobDatasGUID)
                    {
                        mobData = AssetDatabase.LoadAssetAtPath<MobData>(AssetDatabase.GUIDToAssetPath(guid));
                        if (mobData == null) continue;

                        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.MaxHeight(50f), GUILayout.MaxWidth(30f)))
                        {
                            if (GUILayout.Button(EditorGUIUtility.ObjectContent(null, typeof(MobData)).image, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true)))
                            {
                                selectedGUID = guid;
                                originalMob = mobData;
                                copyMob = Instantiate<MobData>(originalMob);
                                SOCopyMob = new SerializedObject(copyMob);
                                state = WindowState.Modify;
                            }

                            EditorGUILayout.LabelField(mobData.name, new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleCenter });
                        }
                    }
                }
            }
        }

        private int currentIndexBehaviour = 0;

        private int PreviousIndexBehaviour => (currentIndexBehaviour - 1 + copyMob.MobFightBehaviourList.Count) % copyMob.MobFightBehaviourList.Count;
        private int CurrentIndexBehaviour { get => currentIndexBehaviour; set => currentIndexBehaviour = value; }
        private int NextIndexBehaviour => (currentIndexBehaviour + 1) % copyMob.MobFightBehaviourList.Count;

        private MobFightBehaviour SelectedMobFightBehaviour => copyMob.MobFightBehaviourList[CurrentIndexBehaviour];
        private List<T> MobActionsCollections<T>()
        {
            return SelectedMobFightBehaviour.MobActionList as List<T>;
        }

        private void Modify()
        {
            //title
            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true)))
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label($"Modify {originalMob.name}", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
            }

            //property drawer
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
            {
                //Base Property
                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true)))
                {
                    using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandWidth(true)))
                    {
                        GUILayout.FlexibleSpace();

                        EditorGUILayout.LabelField("Base Health");
                        copyMob.BaseHealth = Mathf.Max(0, EditorGUILayout.IntField(copyMob.BaseHealth));

                        GUILayout.FlexibleSpace();

                        EditorGUILayout.LabelField("Experience");
                        copyMob.Experience = Mathf.Max(0, EditorGUILayout.IntField(copyMob.Experience));

                        GUILayout.FlexibleSpace();
                    }

                    using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandWidth(true)))
                    {
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.LabelField("Card Lootable");
                        GUILayout.FlexibleSpace();
                    }
                }

                //Behaviour
                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true)))
                {
                    //tool
                    using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandWidth(true)))
                    {
                        GUILayout.FlexibleSpace();

                        GUI.backgroundColor = Color.yellow;
                        if (GUILayout.Button("<"))
                        {
                            SwapBehaviourPlace(CurrentIndexBehaviour, PreviousIndexBehaviour);
                            CurrentIndexBehaviour = PreviousIndexBehaviour;
                        }
                        if (GUILayout.Button(">"))
                        {
                            SwapBehaviourPlace(CurrentIndexBehaviour, NextIndexBehaviour);
                            CurrentIndexBehaviour = NextIndexBehaviour;
                        }

                        GUI.backgroundColor = Color.red;
                        if (GUILayout.Button("-"))
                        {
                            copyMob.MobFightBehaviourList.RemoveAt(CurrentIndexBehaviour);
                            CurrentIndexBehaviour = PreviousIndexBehaviour;
                        }

                        GUI.backgroundColor = Color.green;
                        if (GUILayout.Button("+"))
                        {
                            copyMob.MobFightBehaviourList.Add(new MobFightBehaviour());
                        }

                        GUI.backgroundColor = Color.white;
                        GUILayout.FlexibleSpace();
                    }

                    //list
                    using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandWidth(true)))
                    {
                        GUILayout.FlexibleSpace();
                        GUI.backgroundColor = Color.yellow;
                        if (GUILayout.Button("<"))
                        {
                            CurrentIndexBehaviour = PreviousIndexBehaviour;
                        }

                        GUI.backgroundColor = Color.white;
                        if (copyMob.MobFightBehaviourList != null && copyMob.MobFightBehaviourList.Count > 0)
                        {

                            for (int i = 0; i < copyMob.MobFightBehaviourList.Count; i++)
                            {
                                if (CurrentIndexBehaviour == i)
                                    GUI.enabled = false;

                                if (GUILayout.Button($"{i}"))
                                {
                                    CurrentIndexBehaviour = i;
                                }

                                GUI.enabled = true;
                            }
                        }

                        GUI.backgroundColor = Color.yellow;
                        if (GUILayout.Button(">"))
                        {
                            CurrentIndexBehaviour = NextIndexBehaviour;
                        }
                        GUI.backgroundColor = Color.white;
                        GUILayout.FlexibleSpace();
                    }

                    //fight conditions
                    using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandWidth(true)))
                    {
                        DrawFightConditon(SelectedMobFightBehaviour.PrimaryFightCondition, true);

                        bool canDrawSecondCondition = SelectedMobFightBehaviour.PrimaryFightCondition.ConditionType != FightConditionType.None;
                        if (canDrawSecondCondition)
                        {
                            using (new EditorGUILayout.VerticalScope(GUILayout.Width(50f)))
                            {
                                SelectedMobFightBehaviour.ConditionOperator = (ConditionOperator)EditorGUILayout.EnumPopup(SelectedMobFightBehaviour.ConditionOperator);
                            }
                        }
                        else
                        {
                            GUILayout.Space(50f);
                        }

                        DrawFightConditon(SelectedMobFightBehaviour.SecondaryFightCondition, canDrawSecondCondition);
                    }
                }

                //list mobs
                using (EditorGUILayout.ScrollViewScope scrollView = new EditorGUILayout.ScrollViewScope(scrollPos, EditorStyles.helpBox, GUILayout.ExpandWidth(true)))
                {
                    scrollPos = scrollView.scrollPosition;

                    SerializedProperty mobActionsProp = SOCopyMob.FindProperty("mobBehaviours")
                                                                 .GetArrayElementAtIndex(CurrentIndexBehaviour)
                                                                 .FindPropertyRelative("mobActionList");

                    SOCopyMob.Update();

                    EditorGUILayout.PropertyField(mobActionsProp, new GUIContent("Mob Actions List"), true);

                    SOCopyMob.ApplyModifiedProperties();
                }
            }

            //tool bar
            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true)))
            {
                GUILayout.FlexibleSpace();
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Delete"))
                {
                    AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(selectedGUID));
                    selectedGUID = string.Empty;
                    originalMob = null;
                    currentIndexBehaviour = 0;
                    state = WindowState.Search;
                }
                GUI.backgroundColor = Color.yellow;
                if (GUILayout.Button("Cancel Modification"))
                {
                    selectedGUID = string.Empty;
                    originalMob = null;
                    currentIndexBehaviour = 0;
                    state = WindowState.Search;
                }
                GUI.backgroundColor = Color.green;
                if (GUILayout.Button("Confirm Modification"))
                {
                    //to remove the string add by default by unity
                    //it help the assets database to save it properly with the dirty flag
                    if (copyMob.name.EndsWith("(Clone)"))
                    {
                        copyMob.name = copyMob.name.Replace("(Clone)", "");
                    }

                    EditorUtility.CopySerialized(copyMob, originalMob);
                    EditorUtility.SetDirty(originalMob);
                    AssetDatabase.SaveAssetIfDirty(originalMob);
                    currentIndexBehaviour = 0;
                    state = WindowState.Search;
                }
                GUI.backgroundColor = Color.white;
            }
        }

        private void DrawFightConditon(FightCondition fightCondition, bool drawCondition)
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.MaxWidth(WindowWidth / 2f)))
            {
                if (!drawCondition)
                {
                    GUILayout.Label("Please select a condition in the first Fight Condition before set the second.", new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter, fontSize = 15, wordWrap = true }, GUILayout.Height(EditorGUIUtility.singleLineHeight * 2));
                    return;
                }

                GUILayout.Label("Fight Condition", new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter });

                fightCondition.ConditionType = (FightConditionType)EditorGUILayout.EnumPopup(fightCondition.ConditionType);

                if (fightCondition.ConditionType != FightConditionType.None)
                {
                    if (fightCondition.ConditionType == FightConditionType.MobHealthPercent ||
                        fightCondition.ConditionType == FightConditionType.CharacterHealthPercent ||
                        fightCondition.ConditionType == FightConditionType.MobsCount ||
                        fightCondition.ConditionType == FightConditionType.FightTurn ||
                        fightCondition.ConditionType == FightConditionType.MobOnBoardWithHealth ||
                        fightCondition.ConditionType == FightConditionType.CharacterArmor ||
                        fightCondition.ConditionType == FightConditionType.MobArmor ||
                        fightCondition.ConditionType == FightConditionType.EachXTurns)
                    {
                        using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandWidth(true)))
                        {
                            if (fightCondition.ConditionType != FightConditionType.EachXTurns)
                            {
                                fightCondition.ComparisionOperator = (ComparisonOperator)EditorGUILayout.EnumPopup(fightCondition.ComparisionOperator);
                                GUILayout.Label("Than", new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleCenter });
                            }

                            fightCondition.SpecificValue = EditorGUILayout.FloatField(fightCondition.SpecificValue);
                        }
                    }
                    else if (fightCondition.ConditionType == FightConditionType.CharacterWithState ||
                             fightCondition.ConditionType == FightConditionType.CharacterWithoutState ||
                             fightCondition.ConditionType == FightConditionType.MobsOnBoardWithState ||
                             fightCondition.ConditionType == FightConditionType.MobWithState ||
                             fightCondition.ConditionType == FightConditionType.MobWithoutState)
                    {
                        fightCondition.SpecificStatus = (StatusType)EditorGUILayout.EnumPopup(fightCondition.SpecificStatus);
                    }
                    else if (fightCondition.ConditionType == FightConditionType.MobPosition)
                    {
                        fightCondition.SpecificPosition = (FightPosition)EditorGUILayout.EnumPopup(fightCondition.SpecificPosition);
                    }
                }
            }
        }

        private void SwapBehaviourPlace(int indexA, int indexB)
        {
            if (copyMob.MobFightBehaviourList == null || copyMob.MobFightBehaviourList.Count < 0)
                return;

            int mobBehaviourCount = copyMob.MobFightBehaviourList.Count;
            if (indexA < 0 || indexB < 0 || indexA >= mobBehaviourCount || indexB >= mobBehaviourCount)
                return;

            MobFightBehaviour behaviourA = copyMob.MobFightBehaviourList[indexA];
            copyMob.MobFightBehaviourList[indexA] = copyMob.MobFightBehaviourList[indexB];
            copyMob.MobFightBehaviourList[indexB] = behaviourA;
        }

        private void Create()
        {
            //title
            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(true)))
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("Create", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Exit"))
                {
                    state = WindowState.Main;
                }
            }
        }

        private void CreateAndSaveMob()
        {
            if (mobPrefab == null)
            {
                EditorUtility.DisplayDialog("Erreur", "Veuillez assigner un prefab de base !", "OK");
                return;
            }

            string path = EditorUtility.SaveFolderPanel("Choisir un emplacement", "Assets/", "");
            if (string.IsNullOrEmpty(path)) return;

            string relativePath = "Assets" + path.Substring(Application.dataPath.Length);
            string assetPath = Path.Combine(relativePath, mobName + ".prefab");

            GameObject newMob = Instantiate(mobPrefab);
            newMob.name = mobName;

            // Créer le dossier s'il n'existe pas
            if (!AssetDatabase.IsValidFolder(relativePath))
            {
                Directory.CreateDirectory(path);
                AssetDatabase.Refresh();
            }

            // Sauvegarder en Prefab
            PrefabUtility.SaveAsPrefabAsset(newMob, assetPath);
            DestroyImmediate(newMob);

            EditorUtility.DisplayDialog("Succès", $"Mob sauvegardé dans {assetPath}", "OK");
        }

        private bool LooseFocus()
        {
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                GUI.FocusControl(null);
                Repaint();
                return true;
            }
            return false;
        }
    }
}