namespace OMG.Data.Mobs.Actions
{
    using UnityEngine;
    using MVProduction.CustomAttributes;

    using OMG.Game.Fight.Entities;
    using OMG.Unit.Action;

    [System.Serializable]
    public class MobActionData
    {
        [Title("Mob Action Target")]
        [SerializeField] private MobAction mobAction = null;
        [SerializeField] private FightEntityTarget target = FightEntityTarget.Me;
        [SerializeField] private int value = 0;

        public FightEntityTarget Target { get => target; }

        public void ExecuteAction(params FightEntity[] targets)
        {
            mobAction.Execute(value, targets);
        }
    }
}