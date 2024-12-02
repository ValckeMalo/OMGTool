namespace OMG.Battle.Manager
{
    using OMG.Unit;
    using OMG.Unit.Monster;

    using System.Linq;
    using Unity.Behavior;

    public class MonstersBattleManager
    {
        public MonstersBattleManager(Monster[] monsters, Blackboard blackboard)
        {
            Initialize(monsters, blackboard);
        }
        private void Initialize(Monster[] monsters, Blackboard blackboard)
        {
            foreach (Monster monster in monsters)
            {
                if (monster == null) continue;
                ChooseAction(monster, blackboard);
            }
        }

        public void UpdateMonstersTurn(Monster[] monsters, IUnit oropo, Blackboard blackboard)
        {
            foreach (Monster monster in monsters)
            {
                if (monster == null) continue;

                PlayAction(monster, oropo);
                ChooseAction(monster, blackboard);
            }
        }

        private void ChooseAction(Monster monster, Blackboard blackboard)
        {
            blackboard.Variables.Where(x => x.Name == "ThisMonsterBrain").First().ObjectValue = monster;
            blackboard.Variables.Where(x => x.Name == "ThisMonster").First().ObjectValue = monster;
            monster.SearchNextAction();
        }
        private void PlayAction(Monster monster, IUnit oropo)
        {
            monster.Action(oropo);
        }
    }
}