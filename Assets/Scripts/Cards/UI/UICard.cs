using MaloProduction.CustomAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UICard : MonoBehaviour
{
    [Title("UI")]
    [SerializeField] private Image background;
    [SerializeField] private Image shader;

    [Header("Header Section")]
    [SerializeField] private Image iconValue;
    [SerializeField] private TextMeshProUGUI valueText;

    [SerializeField] private Image wakfu;
    [SerializeField] private TextMeshProUGUI wakfuCostText;

    [Header("Center Section")]
    [SerializeField] private Image icon;

    [Header("Foot Section")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI targetText;
    [SerializeField] private TextMeshProUGUI spellsText;

    public void Init(CardData cardData, CardOptions options)
    {
        background.sprite = options.cardsTypeTexture[(int)cardData.cardType].background;

        //Header Section
        iconValue.sprite = options.cardsTypeTexture[(int)cardData.cardType].iconCard;
        valueText.text = cardData.cardValue.ToString();

        wakfu.sprite = options.wakfu;
        wakfuCostText.text = cardData.wakfuCost.ToString();

        //Center Section
        icon.sprite = cardData.iconCard;

        //Foot Section
        nameText.text = cardData.cardName;
        targetText.text = "Taret : " + cardData.target.ToString();
        spellsText.text = CreateSpellsText(cardData.spells);
    }

    private string CreateSpellsText(List<CardData.SpellsBonus> spellsBonus)
    {
        string spellsString = string.Empty;

        foreach (CardData.SpellsBonus spellBonus in spellsBonus)
        {
            spellsString += spellBonus.spell.ToString(); // get the name of the spell by to string the enum
            spellsString += " : "; // separator
            spellsString += spellBonus.amount.ToString(); // value
            spellsString += "\n"; // line break
        }

        return spellsString;
    }

    public void InitShader(Material shaderMaterial)
    {
        if (shaderMaterial != null)
        {
            shader.gameObject.SetActive(true);
            shader.material = shaderMaterial;
        }
    }
}