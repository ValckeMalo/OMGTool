namespace OMG.Battle.Manager
{
    using OMG.Battle.UI;
    using OMG.Card;
    using OMG.Card.Data;
    using OMG.Card.UI;

    using UnityEngine;

    public class GameBoardManager
    {
        private CardBoardManager cardBoardManager;
        private CardDeckManager cardDeckManager;
        private WakfuManager wakfuManager;

        public GameBoardManager(CardDeck deck)
        {
            cardBoardManager = new CardBoardManager();
            cardDeckManager = new CardDeckManager(deck);
            wakfuManager = new WakfuManager();
        }

        public void StartOropoTurn()
        {
            HUDBattle.EndTurnButton.PlayerTurnButton();
            wakfuManager.TryBreakPadLock();
            SpawnCards(cardBoardManager.NbCardToSpawn);
            cardBoardManager.ToggleCardBasedOnWakfuRemain(wakfuManager.WakfuRemain);
        }
        public void EndOropoTurn()
        {
            wakfuManager.UnlockWakfu();
            HUDBattle.EndTurnButton.MonstersTurnButton();
        }

        public void SpawnCards(int nbCardToSpawn)
        {
            if (cardDeckManager == null || cardDeckManager.IsDeckEmpty || cardBoardManager == null)
            {
                return;
            }

            for (int i = 0; i < nbCardToSpawn; i++)
            {
                SpawnACard();
            }
        }
        private void SpawnACard()
        {
            CardData cardToSpawn = cardDeckManager.GetNextCard();
            PlayableCard playableCard = CardSpawner.OnSpawnCard?.Invoke(cardToSpawn);
            cardBoardManager.AddCardOnBoard(playableCard);
        }

        public void UseCard(PlayableCard playableCard)
        {
            if (playableCard.IsDisable)
            {
                Debug.LogError($"A card that is disable tried to be played : {playableCard.name}.");
                return;
            }

            /*FINISHERS*/
            //TODO test if the card is a finisher and if it pass the turn and destroy the other finishers
            /*\FINISHERS*/

            CardData card = playableCard.Data;
            //process the card and if can be do the rest
            //the card can possibly need to select other card the test is here for that
            if (wakfuManager.CanAddWakfu(card.wakfuCost) && CardUtils.ProcessCard(card, true))
            {
                wakfuManager.AddWakfu(card.wakfuCost);
                cardBoardManager.RemoveCardOnBoard(playableCard);
                cardDeckManager.ReintroduceCard(card);

                cardBoardManager.ToggleCardBasedOnWakfuRemain(wakfuManager.WakfuRemain);

                /*FINISHERS*/
                //TODO try if the condition are good to spawn the finishers
                /*\FINISHERS*/
            }

            //just to help for debug
            Debug.LogWarning($"Card canno't be process");
        }
    }
}