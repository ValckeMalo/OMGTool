namespace OMG.Battle
{
    using OMG.Unit;
    using OMG.Unit.Monster;
    using OMG.Unit.Player;
    using System.Collections;
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

        [SerializeField] private Player player;
        [SerializeField] private Monster[] monsters = new Monster[3];

        private static int turnIndex = 1;
        public static int TurnIndex { get => turnIndex; }

        private void Awake() => OnNextTurn += NextTurn;
        private void Start() => NextTurn(BattleState.Initialize);

        private void NextTurn(BattleState nextState)
        {
            switch (nextState)
            {
                case BattleState.Initialize:
                    StartCoroutine(InitializeCombat());
                    break;

                case BattleState.Player:
                    PlayerBattleSystem.OnUnitTurn?.Invoke();
                    break;

                case BattleState.Monsters:
                    turnIndex++;
                    MonstersBattleSystem.OnUnitTurn?.Invoke();
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
            yield return new WaitUntil(() => InitializeCallBack(PlayerBattleSystem.OnInitialize, player));
            yield return new WaitUntil(() => InitializeCallBack(MonstersBattleSystem.OnInitialize, monsters));

            NextTurn(BattleState.Player);
        }

        private bool InitializeCallBack(UnitBattleSystem.EventInitialize eventInitialize, params IUnit[] units)
        {
            if (eventInitialize == null)
            {
                Debug.LogError($"Can't Initialize {eventInitialize}");
                return false;
            }

            eventInitialize.Invoke(units);
            return true;
        }
    }
}