using static UnityEngine.GraphicsBuffer;
using UnityEditor;
using MaloProduction.Hierachy;

namespace MaloProduction.Hierachy
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [CreateAssetMenu(fileName = "HierarchySettings", menuName = "MV Editor/Hierarchy Settings")]
    public class HierarchySettings : ScriptableObject
    {
        [System.Serializable]
        public class Separator
        {
            public string StartString = "-";
            public Color Color = Color.red;
        }

        public bool Enabled = true;
        public Separator[] Separators;
        public Type[] BlackList()
        {
            return new Type[7] 
            { 
                typeof(LayoutElement), 
                typeof(RectTransform), 
                typeof(Transform), 
                typeof(CanvasRenderer), 
                typeof(VerticalLayoutGroup), 
                typeof(HorizontalLayoutGroup), 
                typeof(ContentSizeFitter), 
            };
        }
    }
}