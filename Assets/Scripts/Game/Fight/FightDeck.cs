namespace OMG.Game.Fight
{
	using System;
	using System.Collections.Generic;

	using UnityEngine;

    using OMG.Data.Card;

    using OMG.Game.Fight.Cards;

    public class FightDeck
	{
		//cards
		private List<FightCard> drawableCard = null;
		private List<FightCard> finishersCard = null;
		private List<FightCard> handCard = null;
		private List<FightCard> securedCard = null;//get all the card when they been hide for perfect request
		
		public Action<FightCard> OnDrawCard;
		public Action<List<FightCard>> OnDrawMultiplesCard;
		public Action OnSecuredAllCards;
		public Action OnUnsecuredAllCards;
		
		private const int MinCardsInHand = 7;
		
		public FightDeck(CardDeck characterDeck)
		{
			drawableCard = new List<FightCard>();
			foreach (CardData cardData in characterDeck.Cards)
			{
				drawableCard.Add(new FightCard(cardData));
			}
			
			finishersCard = new List<FightCard>();
			foreach (CardData cardData in characterDeck.Finishers)
			{
				finishersCard.Add(new FightCard(cardData));
			}

            handCard = new List<FightCard>();
        }
		
		public void NewTurn()
		{
			//get the number of card to draw
			int nbCardToDraw = Mathf.Max(MinCardsInHand - handCard.Count,0);
			
			if (nbCardToDraw > 0)
				DrawCards(nbCardToDraw);
		}
		
		public void DrawCards(int nbToDraw)
		{
			int nbDrawableCard = drawableCard.Count;
			List<FightCard> cardDraw = new List<FightCard>();
			
			for (int i = 0; i < nbToDraw; i++)
			{
				//pick a random card in the drawable one
				FightCard drawFightCard = drawableCard[UnityEngine.Random.Range(0,nbDrawableCard)];
				
				//remove it from the drawable card
				drawableCard.Remove(drawFightCard);
				
				//add it to the hand list
				handCard.Add(drawFightCard);
				
				//add it to the list for the notify after the loop
				cardDraw.Add(drawFightCard);
				
				//minus one to follow the collections and to can't random above the count drawable card
				nbDrawableCard--;
			}
			
			if(cardDraw != null && cardDraw.Count > 0)
			{
				//invoke the action to notify the ui to spawn a new card on HUD
				OnDrawMultiplesCard?.Invoke(cardDraw);
			}
			
			cardDraw.Clear();
			cardDraw = null;
		}
		
		public void DrawSpecificCard(FightCard fightCardToDraw)
		{
			if (fightCardToDraw == null)
				return;
			
			//add it to the hand and notify the ui
			handCard.Add(fightCardToDraw);
			OnDrawCard?.Invoke(fightCardToDraw);
		}
		
		public void AddSpecificDrawableCard(FightCard fightCardToAdd,int nbToAdd)
		{
			if(fightCardToAdd == null || nbToAdd <= 0)
				return;
			
			//add the fight card to the drawable list
			for (int i = 0; i < nbToAdd; i++)
			{
				drawableCard.Add(fightCardToAdd);
			}
		}
		
		public void RemoveCardInHand(FightCard fightCard)
		{
			//remove it from the hand
			handCard.Remove(fightCard);
			
			//only add it back in the drawable pile if the card a not
			//curse divine or also finishers			
			if (fightCard.CardType != CardBackground.Divine && 
				fightCard.CardType != CardBackground.Curse &&
				fightCard.CardType != CardBackground.Finisher)
			{
				drawableCard.Add(fightCard);
			}
			
			//notify the ui that the card is destroyed
			fightCard.OnCardDestroyed?.Invoke();
		}
		
		public void DrawPerfectCard()
		{
			OnSecuredAllCards?.Invoke();
			
			securedCard = handCard;
			handCard.Clear();
			
			OnDrawMultiplesCard?.Invoke(finishersCard);
			
			handCard.AddRange(finishersCard);
		}
		
		public void RemovePerfectCardInHand()
		{
			//remove all the finishers from the hand
			foreach (FightCard perfectCard in handCard)
			{
				RemoveCardInHand(perfectCard);
			}
			
			//readd the previous card before the perfect request back in hand
			OnUnsecuredAllCards?.Invoke();
			
			handCard = securedCard;
			securedCard.Clear();
			securedCard = null;
		}
		
		public void BoostFightCardInHand(FightCard cardToBoost,int valueToAdd)
		{
			cardToBoost.BoostValue(valueToAdd);
		}
		public void BoostAllFightCard(int valueToAdd)
		{
			foreach (FightCard fightCard in handCard)
			{
				if (fightCard == null && !fightCard.IsBoostable) continue;
				fightCard.BoostValue(valueToAdd);
			}
		}
	}
}