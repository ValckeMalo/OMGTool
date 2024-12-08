namespace OMG.Battle.Manager
{
    using MaloProduction.CustomAttributes;

    using OMG.Card.UI;
    using OMG.Card.Data;

    using UnityEngine;
    using System.Collections.Generic;


    [System.Serializable]
    public class CardBoardManager
    {
        [Title("Card Board Manager")]
        [SerializeField] private List<PlayableCard> cardsOnBoard = new List<PlayableCard>();
        private List<PlayableCard> finishersOnBoard = null;
        private const int MaxCardOnBoard = 6;

        public int NbCardToSpawn => Mathf.Max(0, MaxCardOnBoard - cardsOnBoard.Count);

        #region Board
        public bool AddCardOnBoard(PlayableCard playableCard)
        {
            if (playableCard == null || cardsOnBoard == null)
                return false;

            //If the playable is a finisher card put it in his spesific list
            if (playableCard.CardData.background != CardBackground.Finisher)
                cardsOnBoard.Add(playableCard);
            else
                finishersOnBoard.Add(playableCard);

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
                playableCard.UnFixHover(); //to handle some bug
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

            //the array that contain all the finisher that gonna be spawn
            finishersOnBoard = new List<PlayableCard>();
            foreach (CardData finisher in finishers)
            {
                SpawnCardInHand(finisher);
            }
        }
        public void DespawnFinishers()
        {
            ShowAllCards(); //repop all card previously on board

            foreach (PlayableCard finisherOnBoard in finishersOnBoard)
            {
                finisherOnBoard.Destroy();
            }

            finishersOnBoard.Clear();
            finishersOnBoard = null;
        }

        private void SpawnCardInHand(CardData card)
        {
            PlayableCard playableCard = CardSpawner.OnSpawnCard?.Invoke(card);
            AddCardOnBoard(playableCard);
        }
        #endregion
    }
}