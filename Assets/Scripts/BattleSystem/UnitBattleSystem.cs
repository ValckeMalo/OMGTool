namespace OMG.Battle
{
    using OMG.Battle.Data;

    using Unity.Behavior;
    using UnityEngine;

    public abstract class UnitBattleSystem : MonoBehaviour
    {
        public delegate bool EventInitialize(BattleData battleData, Blackboard blackboard);
        public delegate void EventUnitTurn(BattleData battleData);

        protected abstract bool Initialize(BattleData battleData, Blackboard blackboard);
        protected abstract void UnitTurn(BattleData battleData);
    }
}