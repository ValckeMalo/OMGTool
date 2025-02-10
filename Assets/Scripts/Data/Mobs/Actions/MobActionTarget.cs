namespace OMG.Data.Mobs.Actions
{
    using UnityEngine;
    using MVProduction.CustomAttributes;
    using OMG.Unit.Action;

    [System.Serializable]
    public class MobActionTarget
    {
        [Title("Mob Action Target")]
        [SerializeField] private MobAction mobAction = null;
        [SerializeField] private FightEntityTarget target = FightEntityTarget.Me;

        public MobAction MobAction { get => mobAction; }
        public FightEntityTarget Target { get => target; }
    }
}