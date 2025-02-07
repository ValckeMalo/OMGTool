namespace OMG.Card.UI
{
    using MVProduction.Tween;
    using MVProduction.Tween.Core;
    using MVProduction.CustomAttributes;
    using MVProduction.Tween.DoTween.Module;

    using OMG.Battle;
    using OMG.Card.Data;
    using OMG.Battle.UI.Tooltip;

    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    using System;
    using System.Collections;


    public class PlayableCard : UICard, IPointerEnterHandler, IPointerExitHandler
    {
        [Title("Playbale Card")]
        [SerializeField] private Image disableImage;
        [SerializeField] private Button cardButton;

        private int boostValue = 0;
        private int wakfuBoost = 0;

        private RectTransform rect = null;
        private Canvas canvas = null;

        [SerializeField, Range(0.00f, 2.00f)] private float ratioScale = 1.5f;
        private static Vector2 BaseSize = Vector2.zero;

        private TweenerCore<Vector2, Vector2> tweenScale = null;

        private bool isHoverFixed = false;
        private float timeShowTooltip = 1f;

        public int WakfuCost => cardData.wakfu + wakfuBoost;
        public int CardValue => cardData.value + boostValue;
        public CardData CardData => cardData;

        #region Card State
        public void UnusableCard()
        {
            disableImage.enabled = true;
            cardButton.enabled = false;
        }
        public void UsableCard()
        {
            disableImage.enabled = false;
            cardButton.enabled = true;
        }

        public void FixHover()
        {
            isHoverFixed = true;
            ScaleUp(false);
        }
        public void UnFixHover()
        {
            isHoverFixed = false;
            ScaleDown();
        }

        public void HideCard()
        {
            gameObject.SetActive(false);
        }
        public void ShowCard()
        {
            gameObject.SetActive(true);
        }
        #endregion

        public override void Init(CardData cardData, CardOptions options)
        {
            base.Init(cardData, options);

            cardButton.onClick.AddListener(() => BattleSystem.Instance.GameBoard.UseCard(this));//TODO Not illogical but find better way

            base.cardData = cardData;
            rect = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
            BaseSize = rect.sizeDelta;
        }

        public void Destroy()
        {
            TweenManager.Despawn(tweenScale);
            HideTooltipCard();
            Destroy(gameObject);
        }

        public void BoostCardValue(int boostValue)
        {
            this.boostValue += boostValue;
            UpdateUI();
        }
        public void BoostCardWakfu(int wakfuBoost)
        {
            this.wakfuBoost += wakfuBoost;
            UpdateUI();
        }
        private void UpdateUI()
        {
            base.UpdateUI(WakfuCost, CardValue);
        }

        #region Tween
        private void ScaleUp(bool addDelay)
        {
            if (tweenScale != null)
            {
                TweenManager.Despawn(tweenScale);
            }

            if (addDelay)
            {
                tweenScale = rect.DoScale(BaseSize * ratioScale, 0.1f).AddDelay(0.1f);
            }
            else
            {
                tweenScale = rect.DoScale(BaseSize * ratioScale, 0.1f);
            }
        }
        private void ScaleDown()
        {
            if (isHoverFixed) return;

            if (tweenScale != null)
            {
                TweenManager.Despawn(tweenScale);
            }
            rect.DoScale(BaseSize, 0.1f);
        }
        #endregion

        #region IPointer
        public void OnPointerEnter(PointerEventData eventData)
        {
            ScaleUp(true);
            BattleSystem.Instance.GameBoard.UpdatePreviewGauge(WakfuCost);//TODO FIND A BETTER WAY TO LINK UI AND DATA
            StartCoroutine(HoverInfo());
        }

        private IEnumerator HoverInfo()
        {
            yield return new WaitForSeconds(timeShowTooltip);

            Vector2 centerCardCanvas = WorldScreen.UIObjectToCanvasPosition(canvas, rect);
            Vector2 cornerCardCanvas = new Vector2(centerCardCanvas.x - (rect.sizeDelta.x / 2f), centerCardCanvas.y + (rect.sizeDelta.y / 2f));

            TooltipManager.Instance.ShowUnitData(cornerCardCanvas,
                                    0.1f,
                                    TooltipManager.Direction.Left,
                                    TooltipUtils.UnitStateToTooltipData(cardData));
        }
        private void HideTooltipCard()
        {
            StopAllCoroutines();
            TooltipManager.Instance.HideUnitData(0.05f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ScaleDown();
            BattleSystem.Instance.GameBoard.ResetPreviewBar();//TODO FIND A BETTER WAY TO LINK UI AND DATA
            HideTooltipCard();
        }
        #endregion
    }
}