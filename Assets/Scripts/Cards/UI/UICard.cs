using MaloProduction.CustomAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICard : MonoBehaviour
{
    [Title("UI")]
    [Header("UI Image")]
    [SerializeField] private Image background;
    [SerializeField] private Image iconType;
    [SerializeField] private Image icon;
    [SerializeField] private Image wakfu;

    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI nameCard;
    [SerializeField] private TextMeshProUGUI amount;
    [SerializeField] private TextMeshProUGUI wakfuCost;

    public void Init(CardData cardData, CardOptions options)
    {
        background.sprite = options.cardsTypeTexture[(int)cardData.cardType].background;
        iconType.sprite = options.cardsTypeTexture[(int)cardData.cardType].iconCard;
        icon.sprite = cardData.iconCard;
        wakfu.sprite = options.wakfu;

        nameCard.text = cardData.cardName;
        amount.text = cardData.cardValue.ToString();
        wakfuCost.text = cardData.wakfuCost.ToString();
    }
}