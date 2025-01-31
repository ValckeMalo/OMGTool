namespace OMG.Unit.Monster.Brain
{
    using OMG.Unit.Action;
    using OMG.Unit;

    using UnityEngine;
    using Unity.Behavior;

    using MVProduction.CustomAttributes;

    [System.Serializable]
    public class MonsterBrain
    {
        [Title("Barin")]
        [SerializeReference] private BehaviorGraph behaviourTree;
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
            behaviourTree.Start();

            if (nextAction == null)
            {
                Debug.LogError($"The beahaviour tree cannot found any action to play in {behaviourTree.name}.");
            }
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