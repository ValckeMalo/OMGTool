using System.Collections;
using UnityEngine;

namespace OMG.Battle
{
    public enum StateTurn
    {
        Start,
        Player,
        Ennemi,
        Won,
        Lose,
    }

    public class BattleSystem : MonoBehaviour
    {
        public delegate void EventInitializeBattle();
        public delegate void EventNextTurn(StateTurn nextState);
        public delegate void EventEnemiesTurn(EnemyBattleState nextState);
        public delegate void EventPlayerTurn(PlayerBattleState nextState);

        public static EventInitializeBattle OnInitializeBattle;
        public static EventNextTurn OnNextTurn;
        public static EventEnemiesTurn OnEnemiesTurn;
        public static EventPlayerTurn OnPlayerTurn;

        public static int TurnIndex { get; private set; }

        private void Awake()
        {
            TurnIndex = 1;

            OnNextTurn += NextTurnCall;
            OnInitializeBattle += () => StartCoroutine(InitializeCombat());
        }

        private void Start()
        {
            OnNextTurn?.Invoke(StateTurn.Start);
        }

        private void NextTurnCall(StateTurn nextState)
        {
            switch (nextState)
            {
                case StateTurn.Start:
                    OnInitializeBattle?.Invoke();
                    break;

                case StateTurn.Player:
                    OnPlayerTurn?.Invoke(PlayerBattleState.Initialize);
                    break;

                case StateTurn.Ennemi:
                    TurnIndex++;
                    OnEnemiesTurn?.Invoke(EnemyBattleState.PlayAction);
                    break;

                case StateTurn.Won:
                    break;

                case StateTurn.Lose:
                    break;

                default:
                    break;
            }
        }

        private IEnumerator InitializeCombat()
        {
            Debug.Log("Initialize UI");
            yield return new WaitForSeconds(1f);

            OnEnemiesTurn?.Invoke(EnemyBattleState.ChooseAction);
        }
    }
}