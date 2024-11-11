namespace OMG.Unit.Monster.Brain
{
    using OMG.Unit.Action;
    using OMG.Unit;

    using UnityEngine;

    using MaloProduction.BehaviourTree;

    [CreateAssetMenu(fileName = "NewMonsterBrain", menuName = "Unit/Monster/Brain", order = 1)]
    public class MonsterBrain : ScriptableObject
    {
        [SerializeField] private BehaviourTree behaviour;
        [SerializeField] private UnitAction nextAction;

        public void Action(IUnit player, IUnit monster, IUnit[] monsters)
        {
            nextAction.Execute(player, monster, monsters);
        }

        public void SearchAction()
        {
            behaviour.Update();
        }

        public void SetAction(UnitAction unitAction)
        {
            nextAction = unitAction;
        }
    }
}