namespace OMG.Unit.Action
{
    using OMG.Game.Fight.Entities;
    using UnityEngine;

    [CreateAssetMenu(fileName = "PiercingDamageAction", menuName = "Action/Base/Piercing Damage")]
    public class PiercingDamageAction : MobAction
    {
        public override void Execute(int value, params FightEntity[] entities)
        {
            foreach (FightEntity entity in entities)
            {
                entity.LoseHealth(value, true);
            }
        }
    }
}