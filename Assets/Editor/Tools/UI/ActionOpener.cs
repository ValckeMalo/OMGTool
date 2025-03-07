namespace OMG.Tools.UI
{
    using OMG.Data.Mobs.Actions;
    using UnityEditor;
    using UnityEngine;

    public class ActionOpener : AssetPostprocessor
    {
        [UnityEditor.Callbacks.OnOpenAsset(1)]
        static bool OnOpenAsset(int instanceID, int line)
        {
            Object obj = EditorUtility.InstanceIDToObject(instanceID);

            if (obj is MobAction myAsset)
            {
                ActionUITool.OpenWindow(myAsset);
                return true;
            }

            return false;
        }
    }
}