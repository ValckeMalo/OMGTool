using System.Collections;
using UnityEngine;

namespace OMG.Battle
{
    public enum PlayerBattleState
    {
        Initialize,
        Action,
    }

    public class PlayerBattleSystem : MonoBehaviour
    {
        public void Awake()
        {
            BattleSystem.OnPlayerTurn += PlayerTurn;
        }

        private void PlayerTurn(PlayerBattleState state)
        {
            switch (state)
            {
                case PlayerBattleState.Initialize:
                    StartCoroutine(Initialize());
                    break;

                case PlayerBattleState.Action:
                    StartCoroutine(Action());
                    break;

                default:
                    Debug.Log($"State not recognized : {state} in {GetType().Name}.");
                    break;
            }
        }

        private IEnumerator Initialize()
        {
            Debug.Log($"Preparing the player Turn");
            yield return new WaitForSeconds(1f);
            Debug.Log($"Preparation Finished");

            BattleSystem.OnPlayerTurn?.Invoke(PlayerBattleState.Action);
        }

        private IEnumerator Action()
        {
            Debug.Log($"Wait For Action");
            yield return null;
        }
    }
}