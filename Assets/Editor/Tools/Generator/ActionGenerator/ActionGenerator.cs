namespace OMG.Tools.ActionGenerator
{
    using MVProduction;
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    public class ActionGenerator : EditorWindow
    {
        private string ActionName = string.Empty;

        [MenuItem("Tools/Generator/Action Generator")]
        public static void ShowWindow()
        {
            GetWindow<ActionGenerator>("Action Generator");
        }

        public void OnGUI()
        {
            GUILayout.Label("Action Generator", EditorStyles.boldLabel);
            ActionName = EditorGUILayout.TextField("Action Name", ActionName);

            if (GUILayout.Button("Generate Script"))
            {
                if (ActionName != string.Empty)
                {
                    char.ToUpper(ActionName[0]);
                    ActionName = ActionName.CamelCase();

                    GenerateScript();
                }
            }
        }

        private void GenerateScript()
        {
            string ActionTemplate = File.ReadAllText(Path.Combine(Application.dataPath + "/Editor/Tools/Generator/ActionGenerator/ActionTemplate.txt"));
            string formattedScript = ActionTemplate.Replace("{0}", ActionName);
            string path = "Assets/Scripts/Unit/Actions/" + ActionName + "Action.cs";

            File.WriteAllText(path, formattedScript);
            AssetDatabase.Refresh();
        }
    }
}