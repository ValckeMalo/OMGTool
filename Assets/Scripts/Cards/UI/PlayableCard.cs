using MaloProduction.CustomAttributes;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace OMG.Card.UI
{
    public class PlayableCard : UICard
    {
        [Title("Playbale Card")]
        [SerializeField] private Image disableImage;
        [SerializeField] private Button cardButton;
        private CardData data;

        public void DisableCard() => disableImage.enabled = true;
        public void EnableCard() => disableImage.enabled = false;

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

        public override void Init(CardData cardData, CardOptions options)
        {
            base.Init(cardData, options);

            data = cardData;
        }

        /// <summary>
        /// Return if the card has to be removed from the deck
        /// </summary>
        /// <returns></returns>
        public bool Use()
        {
            bool returnValue = false;
            if (data.cardType == CardType.Divine || data.cardType == CardType.Curse)
            {
                returnValue = true;
            }

            Destroy(gameObject);
            return returnValue;
        }
    }
}