namespace OMG.Battle
{
    using OMG.Unit;
    using UnityEngine;

    public abstract class UnitBattleSystem : MonoBehaviour
    {
        public delegate bool EventInitialize(params IUnit[] units);
        public delegate void EventUnitTurn();

        protected abstract bool Initialize(params IUnit[] units);
        protected abstract void UnitTurn();
    }
}