namespace OMG.Battle
{
    using OMG.Unit;
    using OMG.Unit.Monster;
    using OMG.Unit.Player;
    using System.Collections;
    using UnityEngine;

    public enum StateTurn
    {
        Initialize,
        Player,
        Ennemi,
        Won,
        Lose,
    }

    public class BattleSystem : MonoBehaviour
    {
        public delegate void EventNextTurn(StateTurn nextState);
        public static EventNextTurn OnNextTurn;

        [SerializeField] private Player player;
        [SerializeField] private Monster[] monsters = new Monster[3];

        private static int turnIndex = 1;
        public static int TurnIndex { get => turnIndex; }

        private void Awake() => OnNextTurn += NextTurn;
        private void Start() => NextTurn(StateTurn.Initialize);

        private void NextTurn(StateTurn nextState)
        {
            switch (nextState)
            {
                case StateTurn.Initialize:
                    StartCoroutine(InitializeCombat());
                    break;

                case StateTurn.Player:
                    PlayerBattleSystem.OnUnitTurn?.Invoke();
                    break;

                case StateTurn.Ennemi:
                    turnIndex++;
                    MonstersBattleSystem.OnUnitTurn?.Invoke();
                    break;

                case StateTurn.Won:
                    break;

                case StateTurn.Lose:
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

            NextTurn(StateTurn.Player);
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