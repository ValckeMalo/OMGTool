namespace OMG.Battle
{
    using OMG.Battle.Data;
    using OMG.Unit;
    using System.Collections;
    using System.Linq;
    using Unity.Behavior;
    using UnityEngine;

    public enum BattleState
    {
        Initialize,
        Player,
        Monsters,
        Won,
        Lose,
    }

    public class BattleSystem : MonoBehaviour
    {
        public delegate void EventNextTurn(BattleState nextState);
        public static EventNextTurn OnNextTurn;

        [SerializeField] private BattleData battleData;
        [SerializeField] private RuntimeBlackboardAsset blackboardAsset;

        private static int turnIndex = 1;
        public static int TurnIndex { get => turnIndex; }

        private void Awake()
        {
            blackboardAsset.Blackboard.Variables.Where(x => x.Name == "Player").First().ObjectValue = battleData.PlayerData.Data;
            OnNextTurn += NextTurn;
        }

        private void Start() => NextTurn(BattleState.Initialize);

        private void NextTurn(BattleState nextState)
        {
            switch (nextState)
            {
                case BattleState.Initialize:
                    StartCoroutine(InitializeCombat());
                    break;

                case BattleState.Player:
                    PlayerBattleSystem.OnUnitTurn?.Invoke(battleData);
                    break;

                case BattleState.Monsters:
                    turnIndex++;
                    MonstersBattleSystem.OnUnitTurn?.Invoke(battleData);
                    break;

                case BattleState.Won:
                    break;

                case BattleState.Lose:
                    break;

                default:
                    Debug.LogError($"Can't recognize the state send {nextState}.");
                    break;
            }
        }

        private IEnumerator InitializeCombat()
        {
            yield return new WaitUntil(() => InitializeCallBack(PlayerBattleSystem.OnInitialize));
            yield return new WaitUntil(() => InitializeCallBack(MonstersBattleSystem.OnInitialize));

            NextTurn(BattleState.Player);
        }

        private bool InitializeCallBack(UnitBattleSystem.EventInitialize eventInitialize)
        {
            if (eventInitialize == null)
            {
                Debug.LogError($"Can't Initialize {eventInitialize}");
                return false;
            }

            eventInitialize.Invoke(battleData, blackboardAsset.Blackboard);
            return true;
        }
    }
}