namespace OMG.Unit.Action
{
    using UnityEngine;
    using MVProduction.CustomAttributes;

    using OMG.Game.Fight.Entities;
    using OMG.Unit.Status;

    public abstract class MobAction : ScriptableObject
    {
        [Title("Mob Action")]
        [SerializeField] protected UnitActionUI unitActionUI = null;
        public UnitActionUI UnitActionUI => unitActionUI;

        public abstract void Execute(int value, params FightEntity[] entities);
    }
}