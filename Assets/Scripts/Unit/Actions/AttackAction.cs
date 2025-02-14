namespace OMG.Unit.Action
{
    using OMG.Game.Fight.Entities;
    using UnityEngine;

    [CreateAssetMenu(fileName = "AttackAction", menuName = "Action/Base/Attack")]
    public class AttackAction : MobAction
    {
        public override void Execute(int value, params FightEntity[] entities)
        {
            foreach (FightEntity entity in entities)
            {
                entity.LoseHealth(value);
            }
        }
    }
}