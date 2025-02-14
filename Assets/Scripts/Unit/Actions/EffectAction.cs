namespace OMG.Unit.Action
{
    using MVProduction.CustomAttributes;
    using OMG.Game.Fight.Entities;
    using UnityEngine;

    [CreateAssetMenu(fileName = "effectAction", menuName = "Action/Base/Effect")]
    public class EffectAction : MobAction
    {
        [Title("State Action")]
        [SerializeField] private Status.StatusType Effect = Status.StatusType.Poison;

        public override void Execute(int value, params FightEntity[] entities)
        {
            foreach (FightEntity entity in entities)
            {
                entity.AddStatus(Effect, value);
            }
        }
    }
}