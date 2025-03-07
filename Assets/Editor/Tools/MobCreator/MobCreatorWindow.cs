using UnityEngine;
using UnityEditor;
using System.IO;
using OMG.Data.Mobs;
using NUnit.Framework;
using OMG.Data.Mobs.Behaviour;
using System.Collections.Generic;

public class MobCreatorWindow : EditorWindow
{
    private string mobName;
    private GameObject mobPrefab;
	
    private WindowState state = WindowState.Main;
    private string nameFilter = string.Empty;
    
	private MobData originalMob = null;
	private MobData copyMob = null;
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
			using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandHeight(true),GUILayout.ExpandWidth(true)))
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
							state = WindowState.Modify;
						}
						
						EditorGUILayout.LabelField(mobData.name, new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleCenter });
					}
				}
			}
        }
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
					copyMob.BaseHealth = Mathf.Max(0,EditorGUILayout.IntField(copyMob.BaseHealth));
					
					GUILayout.FlexibleSpace();
					
					EditorGUILayout.LabelField("Experience");
					copyMob.Experience = Mathf.Max(0,EditorGUILayout.IntField(copyMob.Experience));
					
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
                state = WindowState.Search;
            }
            GUI.backgroundColor = Color.yellow;
			if (GUILayout.Button("Cancel Modification"))
            {
				selectedGUID = string.Empty;
				originalMob = null;
                state = WindowState.Search;
            }
            GUI.backgroundColor = Color.green;
			if (GUILayout.Button("Confirm Modification"))
            {
				originalMob = copyMob;
				EditorUtility.SetDirty(originalMob);
				AssetDatabase.SaveAssetIfDirty(originalMob);
                state = WindowState.Search;
            }
			GUI.backgroundColor = Color.white;
        }
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