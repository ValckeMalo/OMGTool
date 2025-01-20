namespace OMG.Tools.ModuleGenerator
{
    using System.IO;
    using UnityEditor;
    using UnityEngine;
    using MVProduction;

    public class ModuleGenerator : EditorWindow
    {
        private string typeExtension = "Vector2";
        private string typeT1 = "Vector2";
        private string typeT2 = "Vector2";
        private string className = string.Empty;

        [MenuItem("Tools/Generator/Module Generator")]
        public static void ShowWindow()
        {
            GetWindow<ModuleGenerator>("Module Generator");
        }

        public void OnGUI()
        {
            GUILayout.Label("Script Generator", EditorStyles.boldLabel);
            typeExtension = EditorGUILayout.TextField("Type Extension", typeExtension);
            typeT1 = EditorGUILayout.TextField("Type T1", typeT1);
            typeT2 = EditorGUILayout.TextField("Type T2", typeT2);

            if (GUILayout.Button("Generate Script"))
            {
                if (typeExtension != string.Empty || typeT1 != string.Empty || typeT2 != string.Empty)
                {
                    char.ToUpper(typeExtension[0]);
                    className = typeExtension.CamelCase();
                    GenerateScript();
                }
            }
        }

        private void GenerateScript()
        {
            string ModuleTemplate = File.ReadAllText(Path.Combine(Application.dataPath + "/Editor/Tools/Generator/ModuleGenerator/ModuleTemplate.txt"));
            string formattedScript = ModuleTemplate.Replace("{0}", className).Replace("{1}", typeExtension).Replace("{2}", typeT1).Replace("{3}", typeT2);
            string path = "Assets/Scripts/Tween/DOTween/Modules/" + className + "Module.cs";
            File.WriteAllText(path, formattedScript);
            AssetDatabase.Refresh();
        }
    }
}