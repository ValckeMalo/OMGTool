namespace OMG.Unit.Monster.Brain
{
    using OMG.Unit.Action;
    using OMG.Unit;

    using UnityEngine;
    using Unity.Behavior;

    using MaloProduction.CustomAttributes;

    [CreateAssetMenu(fileName = "NewMonsterBrain", menuName = "Unit/Monster/Brain", order = 1)]
    public class MonsterBrain : ScriptableObject
    {
        [Title("Barin")]
        [SerializeField] private BehaviorGraph behaviourTree;
        [SerializeField] private UnitAction nextAction;

        public void Action(IUnit player, IUnit monster, IUnit[] monsters)
        {
            nextAction.Execute(player, monster, monsters);
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