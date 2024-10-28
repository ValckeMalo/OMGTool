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
        public delegate void EventWakfuUse(int totalAmount, int maxWakfu);
        public static EventWakfuUse OnWakfuUse;

        private int maxWakfu = 3;
        private int wakfuUsed = 0;
        private bool init = true;

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

            InitializeWakfu();

            yield return new WaitForSeconds(1f);
            Debug.Log($"Preparation Finished");

            BattleSystem.OnPlayerTurn?.Invoke(PlayerBattleState.Action);
        }

        private void InitializeWakfu()
        {
            wakfuUsed = 0;
            if (!init)
            {
                maxWakfu++;
                maxWakfu = Mathf.Min(maxWakfu, 6);
            }
            else
            {
                init = false;
            }
            OnWakfuUse?.Invoke(wakfuUsed, maxWakfu);
        }

        private IEnumerator Action()
        {
            Debug.Log($"Wait For Action");
            yield return null;
        }
    }
}