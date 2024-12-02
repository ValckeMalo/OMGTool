namespace OMG.Battle.Manager
{
    using OMG.Card.UI;

    using UnityEngine;
    using System.Collections.Generic;
    using System;

    public class CardBoardManager
    {
        private List<PlayableCard> cardsOnBoard = new List<PlayableCard>();
        private const int MaxCardOnBoard = 6;
        public int NbCardToSpawn => Mathf.Max(0,MaxCardOnBoard - cardsOnBoard.Count);

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
            return playableCard.Data;
        }

        public void HideAllCards()
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

        public void ToggleCardBasedOnWakfuRemain(int wakfuRemain)
        {
            foreach (PlayableCard playableCard in cardsOnBoard)
            {
                if (playableCard.WakfuCost <= wakfuRemain)
                {
                    playableCard.EnableCard();
                }
                else
                {
                    playableCard.DisableCard();
                }
            }
        }

        public void DestroyPlayableCard(PlayableCard playableCard)
        {
            playableCard.Destroy();
        }
    }
}