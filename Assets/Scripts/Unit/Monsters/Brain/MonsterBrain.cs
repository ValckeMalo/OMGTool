namespace OMG.Unit.Monster.Brain
{
    using OMG.Unit.Action;
    using OMG.Unit;

    using UnityEngine;

    using MVProduction.CustomAttributes;

    [System.Serializable]
    public class MonsterBrain
    {
        [Title("Barin")]
        [SerializeField, ReadOnly] private UnitAction nextAction;
        public UnitAction NextAction => nextAction;

        public void Action(IUnit player, IUnit monster)
        {
            if (nextAction == null) return;

            if (nextAction.name.Split("Blindage").Length > 1)
                nextAction.Execute(monster);
            else nextAction.Execute(player);

            nextAction = null;
        }

        public void SearchAction()
        {

        }

        public bool SetAction(UnitAction unitAction)
        {
            if (unitAction == null)
            {
                Debug.LogError($"The unit action {unitAction} passed are null");
                return false;
            }

            nextAction = unitAction;
            return true;
        }
    }
}