namespace OMG.Battle.Manager
{
    using OMG.Unit;
    using OMG.Unit.Monster;

    public class MonstersBattleManager
    {
        public MonstersBattleManager(Monster[] monsters)
        {
            Initialize(monsters);
        }
        private void Initialize(Monster[] monsters)
        {
            foreach (Monster monster in monsters)
            {
                if (monster == null) continue;
                ChooseAction(monster);
            }
        }

        public void UpdateMonstersTurn(Monster[] monsters, IUnit oropo)
        {
            foreach (Monster monster in monsters)
            {
                if (monster == null) continue;

                if (monster.UpdateUnit())
                {
                    PlayAction(monster, oropo);
                    ChooseAction(monster);
                }
            }
        }

        private void ChooseAction(Monster monster)
        {
            monster.SearchNextAction();
        }
        private void PlayAction(Monster monster, IUnit oropo)
        {
            monster.Action(oropo);
        }
    }
}