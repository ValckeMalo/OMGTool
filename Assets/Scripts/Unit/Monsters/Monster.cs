namespace OMG.Unit.Monster
{
    using MaloProduction.CustomAttributes;

    using OMG.Unit;
    using OMG.Unit.Monster.Brain;
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewMonster", menuName = "Unit/Monster/Monster", order = 0)]
    public class Monster : Unit
    {
        [Title("Monster")]
        [SerializeField] private MonsterBrain brain;

        public void Action(IUnit player, IUnit[] monsters)
        {
            if (brain != null)
            {
                brain.Action(player, this, monsters);
            }
        }

        public void SearchNextAction(IUnit player, IUnit[] monsters)
        {
            if (brain != null)
            {
                brain.SearchAction(player, this, monsters);
            }
        }
    }
}