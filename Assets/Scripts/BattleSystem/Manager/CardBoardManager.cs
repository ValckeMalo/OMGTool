namespace OMG.Battle.Manager
{
    using OMG.Card.UI;

    using UnityEngine;
    using System.Collections.Generic;
    using System;

    [System.Serializable]
    public class CardBoardManager
    {
        [SerializeField] private List<PlayableCard> cardsOnBoard = new List<PlayableCard>();
        private const int MaxCardOnBoard = 6;

        public int NbCardToSpawn => Mathf.Max(0, MaxCardOnBoard - cardsOnBoard.Count);

        #region Board
        public bool AddCardOnBoard(PlayableCard playableCard)
        {
            if (playableCard == null || cardsOnBoard == null)
                return false;

            cardsOnBoard.Add(playableCard);
            return true;
        }
        public CardData RemoveCardOnBoard(PlayableCard playableCard)
        {
            if (playableCard == null || !cardsOnBoard.Contains(playableCard))
                return null;

            cardsOnBoard.Remove(playableCard);
            return playableCard.CardData;
        }
        #endregion

        private void HideAllCards()
        {
            foreach (PlayableCard playableCard in cardsOnBoard)
            {
                playableCard.HideCard();
            }
        }
        public void ShowAllCards()
        {
            foreach (PlayableCard playableCard in cardsOnBoard)
            {
                playableCard.ShowCard();
            }
        }

        #region Toggle
        public void ToggleCardBasedOnWakfuRemain(int wakfuRemain)
        {
            foreach (PlayableCard playableCard in cardsOnBoard)
            {
                if (playableCard.WakfuCost <= wakfuRemain)
                {
                    playableCard.UsableCard();
                }
                else
                {
                    playableCard.UnusableCard();
                }
            }
        }
        public void ToggleSacrificiableCard()
        {
            foreach (PlayableCard playableCard in cardsOnBoard)
            {
                playableCard.UsableCard();
            }
        }
        public void ToggleBoostableCard()
        {
            foreach (PlayableCard playableCard in cardsOnBoard)
            {
                if (playableCard.CardData.isBoostable)
                    playableCard.UsableCard();
                else
                    playableCard.UnusableCard();
            }
        }
        #endregion

        public void DestroyPlayableCard(PlayableCard playableCard)
        {
            playableCard.Destroy();
        }

        public void BoostAllCard(int value)
        {
            foreach (PlayableCard card in cardsOnBoard)
            {
                if (card == null) return;
                card.BoostCardValue(value);
            }
        }

        #region Spawn
        public void SpawnFinishers(List<CardData> finishers)
        {
            HideAllCards(); //Hide all card present on board

            if (finishers == null || finishers.Count <= 0)
            {
                Debug.LogError("PB");
                return;
            }

            foreach (CardData finisher in finishers)
            {
                SpawnCardInHand(finisher);
            }
        }
        private void SpawnCardInHand(CardData card)
        {
            PlayableCard playableCard = CardSpawner.OnSpawnCard?.Invoke(card);
            AddCardOnBoard(playableCard);
        }
        #endregion
    }
}