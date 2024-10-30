using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        [Header("Card Deck")]
        [SerializeField] private CardDeck deck;
        [SerializeField] private List<CardData> shuffleDeck;
        private int cardOnBoard = 0;
        private int indexShuffle = 0;

        private int maxWakfu = 3;
        private int wakfuUsed = 0;
        private bool init = true;

        public void Awake()
        {
            BattleSystem.OnPlayerTurn += PlayerTurn;

            shuffleDeck = deck.cards.OrderBy(x => Random.value).ToList();
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

            UpdateWakfu();
            SpawnCardFromDeck();

            yield return new WaitForSeconds(1f);
            Debug.Log($"Preparation Finished");

            BattleSystem.OnPlayerTurn?.Invoke(PlayerBattleState.Action);
        }

        private void UpdateWakfu()
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

        private void SpawnCardFromDeck()
        {
            if (deck == null)
            {
                Debug.Log($"Player doesn't have a Deck in {GetType().Name}.");
                return;
            }

            while (cardOnBoard < 6)
            {
                CardSpawner.OnSpawnCard?.Invoke(shuffleDeck[indexShuffle]);
                cardOnBoard++;
                indexShuffle = (indexShuffle + 1) % shuffleDeck.Count;
            }
        }

        private IEnumerator Action()
        {

            Debug.Log($"Wait For Action");
            yield return null;
        }
    }
}