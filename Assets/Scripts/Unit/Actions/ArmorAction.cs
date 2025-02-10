namespace OMG.Unit.Action
{
    using OMG.Game.Fight.Entities;
    using OMG.Unit;
    using UnityEngine;

    [CreateAssetMenu(fileName = "ArmorAction", menuName = "Action/Base/Armor")]
    public class ArmorAction : MobAction
    {
        public override void Execute(params FightEntity[] entity)
        {
            throw new System.NotImplementedException();
        }
    }
}