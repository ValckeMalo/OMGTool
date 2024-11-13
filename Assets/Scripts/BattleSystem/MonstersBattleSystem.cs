namespace OMG.Battle
{
    using OMG.Unit;
    using OMG.Unit.Monster;
    using OMG.Battle.Data;

    using MaloProduction.CustomAttributes;

    using UnityEngine;
    using Unity.Behavior;
    using System.Linq;
    using OMG.Unit.Monster.Brain;

    public class MonstersBattleSystem : UnitBattleSystem
    {
        public static EventInitialize OnInitialize;
        public static EventUnitTurn OnUnitTurn;

        [SerializeField, ReadOnly] private Monster[] monsters = new Monster[3];
        [SerializeField, ReadOnly] private int nbMonsters = 0;
        private Blackboard blackboard;

        public virtual void Awake()
        {
            OnInitialize += Initialize;
            OnUnitTurn += UnitTurn;
        }

        protected override bool Initialize(BattleData battleData, Blackboard blackboard)
        {
            this.blackboard = blackboard;
            InitializeMonster(battleData.MonstersData);
            ChooseNextAction(battleData.PlayerData);

            return true;
        }
        protected override void UnitTurn(BattleData battleData)
        {
            PlayAction(battleData.PlayerData);
            ChooseNextAction(battleData.PlayerData);
            BattleSystem.OnNextTurn?.Invoke(BattleState.Player);
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

        private void ChooseNextAction(IUnit player)
        {
            for (int i = 0; i < nbMonsters; i++)
            {
                blackboard.Variables.Where(x => x.Type == typeof(MonsterBrain)).First().ObjectValue = monsters[i].Brain;
                blackboard.Variables.Where(x => x.Name == "Monster").First().ObjectValue = monsters[i].Data;
                monsters[i].SearchNextAction(player, monsters);
            }
        }
        private void PlayAction(IUnit player)
        {
            for (int i = 0; i < nbMonsters; i++)
            {
                monsters[i].Action(player, monsters);
            }
        }
    }
}