namespace OMG.Game.Fight.Cards
{
    using MVProduction.Tween;
    using MVProduction.Tween.Core;
    using MVProduction.Tween.DoTween.Module;

    using OMG.Data.Card;
    using OMG.Game.Tooltip;

    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    public class FightCardUI : CardUI, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Header("Data")]
        [SerializeField] private CardUIData cardUIData;
        private FightCard fightCard;
        private FightCardState cardState = FightCardState.Default;

        public FightCardState CardState => cardState;
        public bool IsFightCard(FightCard fightCard) => fightCard == this.fightCard;
        public int CardEnergy => fightCard.Energy;
        public bool IsBoostable => fightCard.IsBoostable;

        [Header("Disable")]
        [SerializeField] private Image disableCardImage;

        //tween animation
        private const float ratioScale = 1.2f;
        private const float timeScale = 0.1f;
        private Vector2 cardSize = Vector2.zero;
        private TweenerCore<Vector2, Vector2> tweenScale = null;

        [Header("Tooltip")]
        [SerializeField] private CanvasGroup tooltipGroup;
        [SerializeField] private Transform containerTooltip;
        private const float tooltipActivationDelay = 0.75f;
        private List<GameObject> allTooltipsObject = null;
        private TweenerCore<float, float> tooltipTween = null;

        public void Initialize(FightCard fightCard, CardSettings cardSettings)
        {
            this.fightCard = fightCard;
            this.fightCard.OnCardUpdated += UpdateFightUI;
            this.fightCard.OnCardDestroyed += OnCardDestroyed;

            cardSize = GetComponent<RectTransform>().sizeDelta;

            SetData(fightCard.Data, cardSettings);
            UpdateCardUI();
            UpdateFightUI();
        }

        #region Action
        private void UpdateFightUI()
        {
            energyText.color = fightCard.HadEnergyBonus ? Color.green : Color.white;
            valueText.color = fightCard.HadValueBonus ? Color.green : Color.white;

            energyText.text = fightCard.Energy.ToString();
            valueText.text = fightCard.Value.ToString();
        }
        private void OnCardDestroyed()
        {
            print("Destroy");
            TweenManager.Despawn(tweenScale);
            TweenManager.Despawn(tooltipTween);

            ReleaseTooltip();

            Destroy(gameObject);
        }
        #endregion

        #region Pointer Event
        public void OnPointerEnter(PointerEventData eventData)
        {
            ScaleUp();
            FightManager.Instance.CardOver(fightCard);
            ShowTooltip();
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            ScaleDown();
            FightManager.Instance.CardExit(fightCard);
            HideTooltip();
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (cardState == FightCardState.Unusable)
                return;

            FightManager.Instance.TryUseCard(fightCard);
        }
        #endregion

        #region Tween
        private void ScaleUp(float delay = 0.1f)
        {
            if (tweenScale != null)
            {
                TweenManager.Despawn(tweenScale);
            }

            tweenScale = cardRect.DoScale(cardSize * ratioScale, timeScale).AddDelay(delay);
        }
        private void ScaleDown()
        {
            if (cardState == FightCardState.Selected) return;

            if (tweenScale != null)
            {
                TweenManager.Despawn(tweenScale);
            }
            cardRect.DoScale(cardSize, timeScale);
        }
        #endregion

        #region Tooltip
        private void SetTooltip(List<TooltipData> allTooltipData)
        {
            List<GameObject> tooltipsObject = TooltipManager.OnSpawnTooltip?.Invoke(allTooltipData);
            if (tooltipsObject != null && tooltipsObject.Count > 0)
            {
                allTooltipsObject = tooltipsObject;
                foreach (GameObject tooltipObject in tooltipsObject)
                {
                    if (tooltipObject == null)
                    {
                        Debug.LogError("One of the tooltip of the entity is null");
                        continue;
                    }

                    tooltipObject.transform.SetParent(containerTooltip, false);
                }
            }
        }
        private void ReleaseTooltip()
        {
            if (allTooltipsObject != null)
            {
                TooltipManager.OnReleaseTooltipObject?.Invoke(allTooltipsObject);
                allTooltipsObject.Clear();
                allTooltipsObject = null;
            }
        }
        private void ShowTooltip()
        {
            if (tooltipTween != null)
                TweenManager.Despawn(tooltipTween);

            SetTooltip(GetCardTooltipsData());
            tooltipTween = tooltipGroup.DoFade(1f, 0.075f).AddDelay(tooltipActivationDelay);
        }
        private List<TooltipData> GetCardTooltipsData()
        {
            if (fightCard == null || fightCard.Data == null)
                return null;

            List<TooltipData> cardTooltipsData = new List<TooltipData>();
            CardData card = fightCard.Data;

            //Add the Sacrifice to the tooltip
            if (card.needSacrifice)
                cardTooltipsData.Add(new TooltipData(TooltipType.CARD, "Sacrifice", cardUIData.NeedSacrificeDesc, null));

            //Add the etheral to the tooltip
            if (card.isEtheral)
                cardTooltipsData.Add(new TooltipData(TooltipType.CARD, "Etheral", cardUIData.EtheralDesc, null));

            //Add the spells in the tooltip
            if (card.spells == null || card.spells.Length <= 0)
                return cardTooltipsData;

            //if multiple spell have initiative, to just add it once
            bool alreadyAddInitiativeDesc = false;
            foreach (CardSpell cardSpell in card.spells)
            {
                if (cardSpell == null)
                    continue;

                SpellType spellType = cardSpell.type;
                CardUIData.CardUIValue cardUIValue = cardUIData.GetValueByKey(spellType);

                if (cardSpell.initiative && !alreadyAddInitiativeDesc)
                {
                    cardTooltipsData.Add(new TooltipData(TooltipType.CARD, "Initiative", cardUIData.InitiativeDesc, null));
                    alreadyAddInitiativeDesc = true;
                }

                cardTooltipsData.Add(new TooltipData(TooltipType.CARD, cardUIValue.title, cardUIValue.description, null));
            }

            return cardTooltipsData;
        }
        private void HideTooltip()
        {
            if (tooltipTween != null)
                TweenManager.Despawn(tooltipTween);

            ReleaseTooltip();
            tooltipTween = tooltipGroup.DoFade(0f, 0.05f);
        }
        #endregion

        #region State
        public void SetState(FightCardState state)
        {
            if (cardState == FightCardState.Secured)
                UnsecuredCard();

            cardState = state;

            UpdateCardState();
        }
        private void UpdateCardState()
        {
            switch (cardState)
            {
                case FightCardState.Secured:
                    SecuredCard();
                    break;

                case FightCardState.Default:
                case FightCardState.Selectable:
                    DefaultCard();
                    ScaleDown();
                    break;

                case FightCardState.Unusable:
                    UnusableCard();
                    break;

                case FightCardState.Selected:
                    ScaleUp();
                    break;

                default:
                    Debug.LogError($"The state throw in the card {this} is not implement or not recognize {cardState}");
                    break;
            }
        }

        private void SecuredCard()
        {
            gameObject.SetActive(false);
        }
        private void UnsecuredCard()
        {
            gameObject.SetActive(true);
        }

        private void DefaultCard()
        {
            disableCardImage.enabled = false;
        }
        private void UnusableCard()
        {
            disableCardImage.enabled = true;
        }
        #endregion
    }
}