namespace OMG.Battle
{
    using OMG.Unit;
    using OMG.Unit.Monster;
    using MaloProduction.CustomAttributes;
    using UnityEngine;

    public class MonstersBattleSystem : UnitBattleSystem
    {
        public static EventInitialize OnInitialize;
        public static EventUnitTurn OnUnitTurn;

        [SerializeField, ReadOnly] private Monster[] monsters = new Monster[3];
        [SerializeField, ReadOnly] private int nbMonsters = 0;

        public virtual void Awake()
        {
            OnInitialize += Initialize;
            OnUnitTurn += UnitTurn;
        }

        protected override bool Initialize(params IUnit[] units)
        {
            InitializeMonster(units);
            ChooseNextAction();

            return true;
        }
        protected override void UnitTurn()
        {
            PlayAction();
            ChooseNextAction();
        }

        private void InitializeMonster(IUnit[] newMonsters)
        {
            nbMonsters = 0;
            foreach (IUnit unit in newMonsters)
            {
                if (unit == null)
                {
                    continue;
                }

                Monster monster = unit as Monster;
                if (monster == null)
                {
                    continue;
                }

                monsters[nbMonsters] = monster;
                nbMonsters++;
            }
        }

        private void ChooseNextAction()
        {
            for (int i = 0; i < nbMonsters; i++)
            {
                monsters[i].SearchNextAction();
            }
        }
        private void PlayAction()
        {
            for (int i = 0; i < nbMonsters; i++)
            {
                monsters[i].Action();
            }
        }
    }
}