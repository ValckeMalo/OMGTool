namespace OMG.Data.Mobs.Actions
{
    using UnityEngine;
    using MVProduction.CustomAttributes;

    using OMG.Game.Fight.Entities;

    [System.Serializable]
    public class MobActionData
    {
        [SerializeField] private MobAction mobAction = null;
        [SerializeField] private FightEntityTarget target = FightEntityTarget.Me;
        [SerializeField] private int value = 0;

        public int Value => value;
        public MobActionUI ActionUI => mobAction.MobActionUI;
        public FightEntityTarget Target { get => target; }

        public void ExecuteAction(params FightEntity[] targets)
        {
            mobAction.Execute(value, targets);
        }
    }
}