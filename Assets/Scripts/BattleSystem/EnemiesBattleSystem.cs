using System.Collections;
using UnityEngine;

namespace OMG.Battle
{
    public enum EnemyBattleState
    {
        ChooseAction,
        PlayAction,
    }

    public class EnemiesBattleSystem : MonoBehaviour
    {
        private Enemy[] enemies = new Enemy[3];

        private void Awake()
        {
            BattleSystem.OnEnemiesTurn += EnemiesTurn;

            enemies[0] = new Bouftou();
            enemies[1] = new Arakne();
        }

        private void EnemiesTurn(EnemyBattleState state)
        {
            switch (state)
            {
                case EnemyBattleState.ChooseAction:
                    StartCoroutine(ChooseAttack());
                    break;

                case EnemyBattleState.PlayAction:
                    StartCoroutine(EnemyActions());
                    break;

                default:
                    Debug.Log($"State not recognized : {state} in {GetType().Name}.");
                    break;
            }
        }

        private IEnumerator ChooseAttack()
        {
            Debug.Log("Enemies are choosing their action");

            foreach (Enemy enemy in enemies)
            {
                if (enemy != null)
                {
                    Debug.Log($"{enemy.GetName()} is choosing his attack");
                    yield return new WaitForSeconds(1f);
                    Debug.Log($"Attack choose");
                }
            }

            BattleSystem.OnNextTurn?.Invoke(StateTurn.Player);
        }

        private IEnumerator EnemyActions()
        {
            foreach (Enemy enemy in enemies)
            {
                if (enemy != null)
                {
                    Debug.Log($"{enemy.GetName()} is doing his action");
                    yield return new WaitForSeconds(2f);
                }
            }

            Debug.Log("All enemy action Played");

            StartCoroutine(ChooseAttack());
        }
    }

    #region Enemy Class
    public abstract class Enemy
    {
        public abstract int Action();

        public abstract string GetName();
    }

    public class Bouftou : Enemy
    {
        public override int Action()
        {
            return 5;
        }

        public override string GetName()
        {
            return "Bouftou";
        }
    }

    public class Arakne : Enemy
    {
        public override int Action()
        {
            return 10;
        }

        public override string GetName()
        {
            return "Arakne";
        }
    }
    #endregion
}