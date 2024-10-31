using System.Collections;
using UnityEngine;

namespace OMG.Battle
{
    using OMG.Enemy;

    public enum EnemyBattleState
    {
        ChooseAction,
        PlayAction,
    }

    public class EnemiesBattleSystem : MonoBehaviour
    {
        private ABSEnemy[] enemies = new ABSEnemy[3];

        public delegate bool EventInitialize();
        public static EventInitialize OnInitialize;

        private void Awake()
        {
            OnInitialize += Initialize;
            BattleSystem.OnEnemiesTurn += EnemiesTurn;

            enemies[0] = new Bouftou();
            enemies[1] = new Arakne();
            enemies[2] = new Bouftou();
        }

        private bool Initialize()
        {
            EnemiesChooseActions();

            return true;
        }

        private void EnemiesTurn(EnemyBattleState state)
        {
            switch (state)
            {
                case EnemyBattleState.ChooseAction:
                    ChooseActions();
                    break;

                case EnemyBattleState.PlayAction:
                    StartCoroutine(EnemyActions());
                    break;

                default:
                    Debug.Log($"State not recognized : {state} in {GetType().Name}.");
                    break;
            }
        }

        private void ChooseActions()
        {
            Debug.Log("Enemies are choosing their action");

            EnemiesChooseActions();
            BattleSystem.OnNextTurn?.Invoke(StateTurn.Player);
        }

        private void EnemiesChooseActions()
        {
            foreach (ABSEnemy enemy in enemies)
            {
                if (enemy != null)
                {
                    Debug.Log($"{enemy.GetName()} is choosing his attack");
                    Debug.Log($"Attack choose");
                }
            }
        }

        private IEnumerator EnemyActions()
        {
            foreach (ABSEnemy enemy in enemies)
            {
                if (enemy != null)
                {
                    Debug.Log($"{enemy.GetName()} is doing his action");
                    yield return new WaitForSeconds(0.5f);
                }
            }

            Debug.Log("All enemy action Played");

            ChooseActions();
        }
    }
}