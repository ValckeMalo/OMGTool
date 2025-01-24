namespace OMG.Unit.Action
{
    using MVProduction.CustomAttributes;

    using UnityEngine;

    [System.Serializable]
    public class UnitActionUI
    {
        [Title("Unit Action UI")]
        [SerializeField,Sprite] private Sprite icon = null;
        [SerializeField] private string name = null;
        [SerializeField,TextArea(5,10)] private string description = null;

        public Sprite Icon => icon;
        public string Name => name;
        public string Description => description;
    }
}