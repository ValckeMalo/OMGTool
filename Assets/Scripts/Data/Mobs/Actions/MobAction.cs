namespace OMG.Data.Mobs.Actions
{
    using UnityEngine;
    using MVProduction.CustomAttributes;

    using OMG.Game.Fight.Entities;

    public abstract class MobAction : ScriptableObject
    {
        [Title("Mob Action")]
        [SerializeField] protected MobActionUI mobActionUI = null;
        public MobActionUI MobActionUI => mobActionUI;

        public abstract void Execute(int value, params FightEntity[] entities);
    }
}