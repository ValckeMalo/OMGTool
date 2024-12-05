namespace OMG.Card.UI
{
    using MaloProduction.CustomAttributes;
    using MaloProduction.Tween.DoTween.Module;
    using MaloProduction.Tween.Core;
    using OMG.Battle;

    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class PlayableCard : UICard, IPointerEnterHandler, IPointerExitHandler
    {
        [Title("Playbale Card")]
        [SerializeField] private Image disableImage;
        [SerializeField] private Button cardButton;

        private CardData data;
        private RectTransform rect;

        private const float ratioScale = 1.5f;
        private static Vector2 BaseSize = Vector2.zero;

        private TweenerCore<Vector2, Vector2> tweenScale = null;

        private bool isHoverFixed = false;

        public int WakfuCost => data.wakfuCost;
        public CardData CardData => data;

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
            ScaleUp();
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

            cardButton.onClick.AddListener(() => BattleSystem.Instance.GameBoard.UseCard(this));

            data = cardData;
            rect = GetComponent<RectTransform>();
            BaseSize = rect.sizeDelta;
        }

        public void Destroy()
        {
            TweenManager.Despawn(tweenScale);
            Destroy(gameObject);
        }

        private void ScaleUp()
        {
            if (tweenScale != null)
            {
                TweenManager.Despawn(tweenScale);
            }
            tweenScale = rect.DoScale(BaseSize * ratioScale, 0.1f);
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

        #region IPointer
        public void OnPointerEnter(PointerEventData eventData)
        {
            ScaleUp();
            BattleSystem.Instance.GameBoard.UpdatePreviewGauge(WakfuCost);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            ScaleDown();
            BattleSystem.Instance.GameBoard.ResetPreviewBar();
        }
        #endregion
    }
}