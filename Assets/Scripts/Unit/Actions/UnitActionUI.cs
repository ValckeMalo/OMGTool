namespace OMG.Unit.Action
{
    using MVProduction.CustomAttributes;

    using UnityEngine;

    [CreateAssetMenu(fileName = "NewUnitActionUI", menuName = "Action/Description", order = -1)]
    public class UnitActionUI : ScriptableObject
    {
        [Title("Unit Action UI")]
        [SerializeField,Sprite] private Sprite icon = null;
        [SerializeField] private new string name = null;
        [SerializeField,TextArea(5,10)] private string description = null;

        public Sprite Icon => icon;
        public string Name => name;
        public string Description => description;
    }
}