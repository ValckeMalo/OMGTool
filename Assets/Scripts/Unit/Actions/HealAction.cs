namespace OMG.Unit.Action
{
    using OMG.Game.Fight.Entities;
    using UnityEngine;

    [CreateAssetMenu(fileName = "HealAction", menuName = "Action/Base/Heal")]
    public class HealAction : MobAction
    {
        public override void Execute(int value, params FightEntity[] entities)
        {
            foreach (FightEntity entity in entities)
            {
                entity.Heal(value);
            }
        }
    }
}