namespace OMG.Data.Mobs.Actions
{
    using OMG.Game.Fight.Entities;
    using UnityEngine;

    [CreateAssetMenu(fileName = "ArmorAction", menuName = "Action/Base/Armor")]
    public class ArmorAction : MobAction
    {
        public override void Execute(int value, params FightEntity[] entities)
        {
            foreach (FightEntity entity in entities)
            {
                entity.AddArmor(value);
            }
        }
    }
}