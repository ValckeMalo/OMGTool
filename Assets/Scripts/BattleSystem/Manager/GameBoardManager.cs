namespace OMG.Battle.Manager
{
    using MaloProduction.CustomAttributes;
    using OMG.Battle.UI;
    using OMG.Card;
    using OMG.Card.Data;
    using OMG.Card.UI;
    using System;
    using UnityEngine;

    [System.Serializable]
    public class GameBoardManager
    {
        [Title("Game Board Manager")]
        [SerializeField] private CardBoardManager cardBoardManager;
        [SerializeField] private CardDeckManager cardDeckManager;
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
            wakfuManager.TryBreakPadLock();
            SpawnCardsInHands(cardBoardManager.NbCardToSpawn);
            cardBoardManager.ToggleCardBasedOnWakfuRemain(wakfuManager.WakfuRemain);
        }
        public void EndOropoTurn()
        {
            wakfuManager.UnlockWakfu();
        }
        #endregion

        #region Cards
        #region SPAWN CARDS
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
        public void AddSpecificCardsInDrawPile(int nbCardToAdd, CardData card)
        {
            for (int i = 0; i < nbCardToAdd; i++)
            {
                AddCardInDrawPile(card);
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
        private void AddCardInDrawPile(CardData card)
        {
            cardDeckManager.AddACardInDrawPile(card);
        }
        #endregion

        #region USE CARD
        public void UseCard(PlayableCard playableCard)
        {
            ProcessPlayableCard(playableCard);

            if (wakfuManager.IsAtMaxWakfu())
            {
                cardBoardManager.SpawnFinishers(cardDeckManager.Finishers);

            }
        }
        private void ProcessPlayableCard(PlayableCard playableCard)
        {
            if (playableCard == null || playableCard.CardData == null || (firstPlayableCard != null && firstPlayableCard == playableCard)) return; //Check

            if (firstPlayableCard != null)
            {
                HUDBattle.Instance.ToggleSelectSecondCard(false);
                ProcessSecondCard(playableCard);
                firstPlayableCard = null;
                return;
            }

            if (wakfuManager.CanAddWakfu(playableCard.WakfuCost)) //If there are enough wakfu available
            {
                if (DoesCardNeedAnotherCard(playableCard.CardData)) //Search if the card does need another one
                {
                    playableCard.FixHover(); //Let it in hover mode
                    cardBoardManager.ToggleSacrificiableCard();
                    firstPlayableCard = playableCard;
                    HUDBattle.Instance.ToggleSelectSecondCard(true);
                    return;
                }
                else
                {
                    CardUtils.ProcessCard(playableCard.CardData, playableCard.CardValue, false);
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
            wakfuManager.AddWakfu(playableCard.WakfuCost);

            DestroyCardOnBoard(playableCard);

            cardDeckManager.ReintroduceCard(playableCard.CardData);

            cardBoardManager.ToggleCardBasedOnWakfuRemain(wakfuManager.WakfuRemain);
        }
        private void DestroyCardOnBoard(PlayableCard playableCard)
        {
            cardBoardManager.RemoveCardOnBoard(playableCard);
            cardBoardManager.DestroyPlayableCard(playableCard);
        }
        private void ProcessSecondCard(PlayableCard secondPlayableCard)
        {
            CardData secondCard = secondPlayableCard.CardData;
            if (firstPlayableCard.CardData.needSacrifice)
            {
                ProcessSecondCardSacrifice(secondPlayableCard);
            }
            else if (firstPlayableCard.CardData.cardType == CardType.BoostSingle)
            {
                ProcessSecondCardBoost(secondPlayableCard);
            }
        }
        private void ProcessSecondCardSacrifice(PlayableCard secondPlayableCard)
        {
            DestroyCardOnBoard(secondPlayableCard);
            cardDeckManager.ReintroduceCard(secondPlayableCard.CardData);

            CardUtils.ProcessCard(firstPlayableCard.CardData, firstPlayableCard.CardValue, false);

            ProcessCard(firstPlayableCard);
        }
        private void ProcessSecondCardBoost(PlayableCard secondPlayableCard)
        {
            secondPlayableCard.BoostCardValue(firstPlayableCard.CardValue);

            CardUtils.ProcessOnlyCardSpells(firstPlayableCard.CardData, false);
            ProcessCard(firstPlayableCard);
        }
        public void CancelSecondCard()
        {
            if (firstPlayableCard == null) return;

            firstPlayableCard.UnFixHover();
            firstPlayableCard = null;

            cardBoardManager.ToggleCardBasedOnWakfuRemain(wakfuManager.WakfuRemain);
            wakfuManager.ResetPreviewBar();
        }
        public void BoostAllCard(int value)
        {
            cardBoardManager.BoostAllCard(value); ;
        }
        #endregion
        #endregion

        #region Wakfu
        public void UpdatePreviewGauge(int amount)
        {
            if (firstPlayableCard != null) return;
            wakfuManager.PreviewWakfu(amount);
        }
        public void ResetPreviewBar()
        {
            if (firstPlayableCard != null) return;
            wakfuManager.ResetPreviewBar();
        }
        #endregion
    }
}