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

        private PlayableCard firstPlayableCard = null;


        public GameBoardManager(CardDeck deck)
        {
            cardBoardManager = new CardBoardManager();
            cardDeckManager = new CardDeckManager(deck);
            wakfuManager = new WakfuManager();
        }

        #region OropoTurn
        public void StartOropoTurn()
        {
            HUDBattle.EndTurnButton.PlayerTurnButton();
            wakfuManager.TryBreakPadLock();
            SpawnCardsInHands(cardBoardManager.NbCardToSpawn);
            cardBoardManager.ToggleCardBasedOnWakfuRemain(wakfuManager.WakfuRemain);
        }
        public void EndOropoTurn()
        {
            wakfuManager.UnlockWakfu();
            HUDBattle.EndTurnButton.MonstersTurnButton();
        }
        #endregion

        #region Cards
        public void SpawnCardsInHands(int nbCardToSpawn)
        {
            if (cardDeckManager == null || cardDeckManager.IsDeckEmpty || cardBoardManager == null)
            {
                return;
            }

            for (int i = 0; i < nbCardToSpawn; i++)
            {
                SpawnACardInHand();
            }
        }
        public void SpawnSpecificCardsInHand(int nbCardToSpawn, CardData card)
        {
            for (int i = 0; i < nbCardToSpawn; i++)
            {
                SpawnSpecificCardInHand(card);
            }
        }
        public void AddSpecificCardsInDeck(int nbCardToAdd, CardData card)
        {
            for (int i = 0; i < nbCardToAdd; i++)
            {
                //TODO
            }
        }
        private void SpawnACardInHand()
        {
            CardData cardToSpawn = cardDeckManager.GetNextCard();
            PlayableCard playableCard = CardSpawner.OnSpawnCard?.Invoke(cardToSpawn);
            cardBoardManager.AddCardOnBoard(playableCard);
        }
        private void SpawnSpecificCardInHand(CardData card)
        {
            PlayableCard playableCard = CardSpawner.OnSpawnCard?.Invoke(card);
            cardBoardManager.AddCardOnBoard(playableCard);
        }
        public void RemoveCardOnBoard(PlayableCard playableCard)
        {
            if (playableCard == null) return;

            cardBoardManager.RemoveCardOnBoard(playableCard);
        }

        //USE CARD
        public void UseCard(PlayableCard playableCard)
        {
            if (playableCard == null || playableCard.CardData == null || (firstPlayableCard != null && firstPlayableCard == playableCard)) return; //Check

            if (firstPlayableCard != null)
            {
                ProcessCardSecondCard(playableCard);
                firstPlayableCard = null;
                return;
            }

            if (wakfuManager.CanAddWakfu(playableCard.CardData.wakfuCost)) //If there are enough wakfu available
            {
                if (DoesCardNeedAnotherCard(playableCard.CardData)) //Search if the card does need another one
                {
                    playableCard.SwitchState(CardState.Hover);
                    firstPlayableCard = playableCard;
                    return;
                }
                else
                {
                    CardUtils.ProcessCard(playableCard.CardData, false);
                    ProcessCard(playableCard);
                }
            }
        }
        private bool DoesCardNeedAnotherCard(CardData card)
        {
            return (card.needSacrifice || card.cardType == CardType.BoostSingle);
        }
        private void ProcessCard(PlayableCard playableCard)
        {
            wakfuManager.AddWakfu(playableCard.CardData.wakfuCost);

            DestroyCardOnBoard(playableCard);

            //TODO IS A TEMP CARD
            cardDeckManager.ReintroduceCard(playableCard.CardData);

            cardBoardManager.ToggleCardBasedOnWakfuRemain(wakfuManager.WakfuRemain);
        }
        private void DestroyCardOnBoard(PlayableCard playableCard)
        {
            cardBoardManager.RemoveCardOnBoard(playableCard);
            cardBoardManager.DestroyPlayableCard(playableCard);
        }
        private void ProcessCardSecondCard(PlayableCard secondPlayableCard)
        {
            CardData secondCard = secondPlayableCard.CardData;

            if (firstPlayableCard.CardData.needSacrifice)
            {
                DestroyCardOnBoard(secondPlayableCard);
                cardDeckManager.ReintroduceCard(secondCard);

                CardUtils.ProcessCard(firstPlayableCard.CardData, false);

                ProcessCard(firstPlayableCard);
            }
            else if (firstPlayableCard.CardData.cardType == CardType.BoostSingle)
            {
                //TODO BOOST
            }
        }
        /////////////
        #endregion

        #region Wakfu
        public void UpdatePreviewGauge(int amount) => wakfuManager.PreviewWakfu(amount);
        public void ResetPreviewBar()
        {
            wakfuManager.ResetPreviewBar();
        }
        #endregion
    }
}