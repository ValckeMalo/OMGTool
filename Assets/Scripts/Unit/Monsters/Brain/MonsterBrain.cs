namespace OMG.Unit.Monster.Brain
{
    using OMG.Unit.Action;
    using OMG.Unit;

    using UnityEngine;
    using Unity.Behavior;

    using MaloProduction.CustomAttributes;

    [System.Serializable]
    public class MonsterBrain
    {
        [Title("Barin")]
        [SerializeReference] private BehaviorGraph behaviourTree;
        [SerializeField, ReadOnly] private UnitAction nextAction;

        public void Action(IUnit player, IUnit monster, IUnit[] monsters)
        {
            if (nextAction == null) return;

            nextAction.Execute(player);
            nextAction = null;
        }
        public void Action(IUnit player, IUnit monster)
        {
            if (nextAction == null) return;

            Debug.Log($"Action play is {nextAction.name}");
            nextAction.Execute(player);
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