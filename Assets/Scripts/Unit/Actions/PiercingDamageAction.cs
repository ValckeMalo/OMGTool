namespace OMG.Unit.Action
{
    using OMG.Game.Fight.Entities;
    using OMG.Unit;
    using UnityEngine;

    [CreateAssetMenu(fileName = "PiercingDamageAction", menuName = "Action/Base/Piercing Damage")]
    public class PiercingDamageAction : MobAction
    {
        public override void Execute(params FightEntity[] entity)
        {
            throw new System.NotImplementedException();
        }
    }
}