namespace OMG.Unit.Monster
{
    using MaloProduction.CustomAttributes;
    using OMG.Battle;
    using OMG.Unit;
    using OMG.Unit.Monster.Brain;
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewMonster", menuName = "Unit/Monster/Monster", order = 0)]
    public class Monster : Unit
    {
        [Title("Monster")]
        [SerializeField] private MonsterBrain brain;
        public MonsterBrain Brain { get => brain; }

        public void Action(IUnit player)
        {
            if (brain != null)
            {
                brain.Action(player, this);
            }
        }

        public void SearchNextAction()
        {
            if (brain != null)
            {
                brain.SearchAction();
                unitHUD.UpdatePreviewNextAttack(brain.NextActionValue);
            }
        }

        protected override void Death()
        {
            BattleSystem.Instance.MobDead(this);
        }
    }
}