namespace OMG.Battle
{
    using OMG.Unit;
    using OMG.Unit.Monster;
    using OMG.Battle.Data;

    using MaloProduction.CustomAttributes;
    using MaloProduction.BehaviourTree.Blackboard;

    using UnityEngine;

    public class MonstersBattleSystem : UnitBattleSystem
    {
        public static EventInitialize OnInitialize;
        public static EventUnitTurn OnUnitTurn;

        [SerializeField, ReadOnly] private Monster[] monsters = new Monster[3];
        [SerializeField, ReadOnly] private int nbMonsters = 0;

        private Blackboard sharedBlackboard;

        public virtual void Awake()
        {
            OnInitialize += Initialize;
            OnUnitTurn += UnitTurn;
        }

        protected override bool Initialize(BattleData battleData)
        {
            InitializeMonster(battleData.MonstersData);
            ChooseNextAction(battleData.PlayerData);

            sharedBlackboard = ScriptableObject.CreateInstance<Blackboard>();
            sharedBlackboard.GetBlackboardValues.Add(new("Player", battleData.PlayerData));
            sharedBlackboard.GetBlackboardValues.Add(new("Monsters", battleData.MonstersData));
            sharedBlackboard.GetBlackboardValues.Add(new("Monster", null));

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