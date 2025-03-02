namespace OMG.Game.Fight
{
    using System;
    using System.Collections.Generic;
	
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    
    using OMG.Data.Card;
    using OMG.Game.Fight.Cards;
	
    using MVProduction.CustomAttributes;
    
    using MVProduction.Tween;
    using MVProduction.Tween.Core;
    using MVProduction.Tween.DoTween.Module;

    public class FightUI : MonoBehaviour
	{
		[Title("UI")]		
		[Header("Cards")]
		[SerializeField] private Transform cardsContainer;
        [SerializeField] private GameObject fightCardUIPrefab;
        [SerializeField] private CardSettings cardSettings;
		private List<FightCardUI> handsUI = new List<FightCardUI>();
		private List<FightCardUI> securedCardsUI = null;

		[Header("Select Card")]
		[SerializeField] private CanvasGroup cardSelectGroup;
		[SerializeField] private GameObject cardSelectObject;
		[SerializeField] private Button cardSelectButton;
		public Action OnCancelCardSelect;
		private const float cardSelectFadeDuration = 0.2f;
        private TweenerCore<float, float> cardSelectTween = null;
		
		[Header("Energy")]
        [SerializeField] private Slider energySlider;
        [SerializeField] private Slider previewEnergySlider;
        [SerializeField] private Image[] padlockImages = new Image[3];
        [SerializeField] private Sprite[] padlockSprite = new Sprite[2];
        private TweenerCore<float, float> previewEnergyTween;
		
		[Header("Turn Button")]
		[SerializeField] private Button turnButton;
        [SerializeField] private TextMeshProUGUI entityTurnText;
        [SerializeField] private TextMeshProUGUI turnCountText;
		public Action OnRequestCharacterEndTurn;
		
		[Header("Turn Banner")]
		[SerializeField] private CanvasGroup turnBannerGroup;
		[SerializeField] private RectTransform turnBannerTransform;
		[SerializeField] private TextMeshProUGUI turnBannerText;
		private const float turnBannerStay = 1.5f;
		private const float turnBannerSlide = 0.25f;
		private const float turnBannerFade = 0.25f;
		
		public void Initialize(FightDeck fightDeck)
		{
			fightDeck.OnDrawCard += DrawCard;
			fightDeck.OnDrawMultiplesCard += DrawMultiplesCard;
			fightDeck.OnSecuredAllCards += SecuredAllCards;
			fightDeck.OnUnsecuredAllCards += UnsecuredAllCards;
		
			InitializeCardSelect();
			InitializeTurnButton();
		}
		
		#region Cards
		
		#region Draw
		private void DrawCard(FightCard drawCard)
		{
			if (drawCard == null)
				return;

            if (drawCard.Data == null)
            {
                Debug.LogError("The card draw is equal to null");
                return;
            }

            GameObject cardObject = Instantiate(fightCardUIPrefab, cardsContainer);
			cardObject.name = drawCard.Data.name;
			
			FightCardUI fightCardUI = cardObject.GetComponent<FightCardUI>();
			fightCardUI.Initialize(drawCard,cardSettings);
		}
		private void DrawMultiplesCard(List<FightCard> drawCards)
		{
			if (drawCards == null || drawCards.Count <= 0)
				return;
			
			foreach (FightCard fightCard in drawCards)
			{
				DrawCard(fightCard);
			}
		}
		#endregion
		
		#region Secured
		private void SecuredAllCards()
		{
			if (handsUI.Count <= 0)
				return;
			
			if (securedCardsUI != null)
				securedCardsUI.Clear();
			else
				securedCardsUI = new List<FightCardUI>();
			
			//add the card in the secured collection
			securedCardsUI.AddRange(handsUI);

			//secured all card
			foreach (FightCardUI cardUI in securedCardsUI)
			{
				if (cardUI == null) continue;
				cardUI.SetState(FightCardState.Secured);
			}
		}
		private void UnsecuredAllCards()
		{
			if (securedCardsUI == null || securedCardsUI.Count <= 0)
				return;
			
			//readd the card in the hand collection
			handsUI.AddRange(securedCardsUI);

			foreach (FightCardUI cardUI in securedCardsUI)
			{
				if (cardUI == null) continue;
				cardUI.SetState(FightCardState.Default);
			}
		}
		#endregion
		
		#region State
		public void SetCardStateOnEnergy(int remainEnergy)
		{
			if (handsUI == null || handsUI.Count <= 0)
				return;
			
			foreach (FightCardUI fightCardUI in handsUI)
			{
				if (fightCardUI == null) continue;
				
				if (fightCardUI.CardEnergy <= remainEnergy)
				{
					fightCardUI.SetState(FightCardState.Default);
				}
				else
				{
					fightCardUI.SetState(FightCardState.Unusable);
				}
			}
		}
		public void SetSelectableBoostableCard()
		{
			if (handsUI == null || handsUI.Count <= 0)
				return;
			
			foreach (FightCardUI fightCardUI in handsUI)
			{
				if (fightCardUI == null) continue;
				
				if (fightCardUI.IsBoostable)
				{
					fightCardUI.SetState(FightCardState.Selectable);
				}
				else
				{
					fightCardUI.SetState(FightCardState.Unusable);
				}
			}
		}
		public void SetSacrificialbeCard()
		{
			if (handsUI == null || handsUI.Count <= 0)
				return;
			
			foreach (FightCardUI fightCardUI in handsUI)
			{
				if (fightCardUI == null) continue;

				fightCardUI.SetState(FightCardState.Selectable);
			}			
		}
		#endregion
		
		#endregion
		
		#region Select Cards
		private void InitializeCardSelect()
		{
			cardSelectObject.SetActive(false);
			cardSelectButton.onClick.AddListener(() => OnCancelCardSelect?.Invoke());
		}
		public void ShowCardSelectUI()
		{
			if (cardSelectTween != null)
				TweenManager.Despawn(cardSelectTween);
			
			cardSelectTween = cardSelectGroup.DoFade(1f,cardSelectFadeDuration);
			cardSelectObject.SetActive(true);
		}
		public void HideCardSelectUI()
		{
			if (cardSelectTween != null)
				TweenManager.Despawn(cardSelectTween);
			
			cardSelectTween = cardSelectGroup.DoFade(0f,cardSelectFadeDuration);
			cardSelectObject.SetActive(false);
		}
		#endregion
		
		#region Energy
		public void UpdateCurrentEnergyUI(int currentEnergy)
		{			
			energySlider.value = currentEnergy;
		}
		public void UpdatePreviewEnergyUI(int previewEenergy)
		{
			if (previewEnergyTween != null)
				TweenManager.Despawn(previewEnergyTween);
			
			previewEnergyTween = previewEnergySlider.DoValue((float)previewEenergy,0.2f);
		}
		public void BreakPadlock(int padlockPos)
		{
			padlockImages[padlockPos].sprite = padlockSprite[1];
		}
		public void RemovePadlock(int padlockPos)
		{
			padlockImages[padlockPos].gameObject.SetActive(false);
		}
		public void ResetPadlock()
		{
			foreach (Image padlockImage in padlockImages)
			{
				padlockImage.gameObject.SetActive(true);
				padlockImage.sprite = padlockSprite[0];
			}
		}
		#endregion
	
		#region Turn Button
		private void InitializeTurnButton()
		{
            StartCharacterTurnButton(1);
			turnButton.onClick.AddListener(() => OnRequestCharacterEndTurn?.Invoke());
		}
		public void StartMobsTurnButton()
		{
			SetTurnButtonInteractible(false);
			entityTurnText.text = "Opponent Turn";
		}
		public void StartCharacterTurnButton(int turnCount)
		{
			SetTurnButtonInteractible(true);
			UpdateTurnCount(turnCount);
			entityTurnText.text = "End Turn";
		}
		private void UpdateTurnCount(int turnCount)
		{
			turnCountText.text = "TURN " + turnCount.ToString();
		}
		private void SetTurnButtonInteractible(bool state)
		{
			turnButton.interactable = state;
		}
		#endregion
		
		#region Perfect
		public void SetPerfectMode()
		{
			//TODO toggle perfect text on the energy bar
			SetTurnButtonInteractible(false);
		}
		#endregion
		
		#region Turn Banner
		public void PlayBannerTurnMobs()
		{			
			turnBannerText.text = "Player Turn";
			TurnBannerTween(-500f);
		}
		public void PlayBannerTurnCharacter()
		{
			turnBannerText.text = "Opponent Turn";
			TurnBannerTween(500f);
		}
		private void TurnBannerTween(float finalPosX)
		{
			turnBannerGroup.DoFade(1f,turnBannerFade);
			turnBannerTransform.DoAnchorMove(new Vector2(0f, turnBannerTransform.anchoredPosition.y), turnBannerSlide)
				.OnComplete(
					() =>
					{
						turnBannerTransform.DoAnchorMove(new Vector2(finalPosX,turnBannerTransform.anchoredPosition.y),turnBannerSlide)
							.AddDelay(turnBannerStay);
						turnBannerGroup.DoFade(0f,turnBannerFade);
					});
		}
		#endregion	
	}
}