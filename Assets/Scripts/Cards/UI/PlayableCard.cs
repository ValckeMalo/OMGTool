namespace OMG.Card.UI
{
    using MaloProduction.CustomAttributes;
    using MaloProduction.Tween.DoTween.Module;
    using OMG.Battle;
    using OMG.Battle.UI;

    using System;
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
        private static Vector2 size;

        public void DisableCard() => disableImage.enabled = true;
        public void EnableCard() => disableImage.enabled = false;
        public void HideCard() => gameObject.SetActive(false);
        public void ShowCard() => gameObject.SetActive(true);

        public bool RegisterOnClick(Action action)
        {
            if (cardButton == null)
            {
                Debug.LogError($"The button of the card are not assigned");
                return false;
            }

            cardButton.onClick.AddListener(() => action());

            return true;
        }

        public int Wakfu { get => data.wakfuCost; }
        public bool IsDisable { get => disableImage.enabled; }
        public CardData Data { get => data; }
        public CardType Type { get => data.cardType; }

        public override void Init(CardData cardData, CardOptions options)
        {
            base.Init(cardData, options);

            data = cardData;
            rect = GetComponent<RectTransform>();
            size = rect.sizeDelta;
        }

        /// <summary>
        /// Return if the card has to be removed from the deck
        /// </summary>
        /// <returns></returns>
        public bool Use()
        {
            //TODO rework of the function
            bool returnValue = false;
            if (data.cardType == CardType.Divine || data.cardType == CardType.Curse)
            {
                returnValue = true;
            }

            Destroy(gameObject);
            return returnValue;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        #region IPointer
        public void OnPointerEnter(PointerEventData eventData)
        {
            rect.DoScale(size * ratioScale, 0.1f);
            PlayerBattleSystem.UpdatePreviewGauge(Wakfu);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            rect.DoScale(size, 0.1f);
            HUDBattle.OropoWakfuGauge.ResetPreviewBar();
        }
        #endregion
    }
}