namespace OMG.Battle
{
    using OMG.Battle.Data;

    using UnityEngine;

    public abstract class UnitBattleSystem : MonoBehaviour
    {
        public delegate bool EventInitialize(BattleData battleData);
        public delegate void EventUnitTurn(BattleData battleData);

        protected abstract bool Initialize(BattleData battleData);
        protected abstract void UnitTurn(BattleData battleData);
    }
}