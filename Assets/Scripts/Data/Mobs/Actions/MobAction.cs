namespace OMG.Data.Mobs.Actions
{
    using UnityEngine;
    using MVProduction.CustomAttributes;

    using OMG.Game.Fight.Entities;

    public abstract class MobAction : ScriptableObject
    {
        [SerializeField] protected MobActionUI mobActionUI = null;
        public MobActionUI MobActionUI => mobActionUI;

        public abstract void Execute(int value, params FightEntity[] entities);
    }
}