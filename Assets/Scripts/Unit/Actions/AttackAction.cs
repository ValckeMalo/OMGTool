namespace OMG.Unit.Action
{
    using OMG.Game.Fight.Entities;
    using OMG.Unit;
    using UnityEngine;

    [CreateAssetMenu(fileName = "AttackAction", menuName = "Action/Base/Attack")]
    public class AttackAction : MobAction
    {
        public override void Execute(params FightEntity[] entity)
        {
            throw new System.NotImplementedException();
        }
    }
}