namespace OMG.Card.UI
{
    using MaloProduction.CustomAttributes;
    using MaloProduction.Tween.DoTween.Module;
    using OMG.Battle;
    using MaloProduction.Tween.Core;

    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public enum CardState
    {
        Normal,
        Hover,
        Unusable,
        Destroy,
        Hide,
    }

    public class PlayableCard : UICard, IPointerEnterHandler, IPointerExitHandler
    {
        [Title("Playbale Card")]
        [SerializeField] private Image disableImage;
        [SerializeField] private Button cardButton;

        private CardData data;
        private RectTransform rect;
        private const float ratioScale = 1.5f;
        private static Vector2 size;
        private TweenerCore<Vector2, Vector2> tweenScale;
        private CardState state = CardState.Normal;

        public void DisableCard() => disableImage.enabled = true;
        public void EnableCard() => disableImage.enabled = false;
        public void HideCard() => gameObject.SetActive(false);
        public void ShowCard() => gameObject.SetActive(true);

        public void SwitchState(CardState newState)
        {
            state = newState;
            switch (newState)
            {
                case CardState.Normal:
                    ScaleDown();
                    break;

                case CardState.Hover:
                    ScaleUp();
                    break;

                case CardState.Unusable:
                    DisableCard();
                    break;

                case CardState.Destroy:
                    break;

                case CardState.Hide:
                    HideCard();
                    break;

                default:
                    break;
            }
        }

        public int WakfuCost { get => data.wakfuCost; }
        public CardData CardData { get => data; }

        public override void Init(CardData cardData, CardOptions options)
        {
            base.Init(cardData, options);

            cardButton.onClick.AddListener(() => BattleSystem.Instance.GameBoard.UseCard(this));

            data = cardData;
            rect = GetComponent<RectTransform>();
            size = rect.sizeDelta;
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
            tweenScale = rect.DoScale(size * ratioScale, 0.1f);
        }
        private void ScaleDown()
        {
            if (tweenScale != null)
            {
                TweenManager.Despawn(tweenScale);
            }
            rect.DoScale(size, 0.1f);
        }

        #region IPointer
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (state == CardState.Hover) return;

            ScaleUp();
            BattleSystem.Instance.GameBoard.UpdatePreviewGauge(WakfuCost);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            if (state == CardState.Hover) return;

            ScaleDown();
            BattleSystem.Instance.GameBoard.ResetPreviewBar();
        }
        #endregion
    }
}