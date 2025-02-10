namespace OMG.Unit.Action
{
    using MVProduction.CustomAttributes;
    using OMG.Game.Fight.Entities;
    using OMG.Unit;
    using UnityEngine;

    [CreateAssetMenu(fileName = "effectAction", menuName = "Action/Base/Effect")]
    public class EffectAction : MobAction
    {
        //[Title("State Action")]
        //[SerializeField] private Status.StatusType Effect = Status.StatusType.Poison;

        public override void Execute(params FightEntity[] entity)
        {
            throw new System.NotImplementedException();
        }
    }
}