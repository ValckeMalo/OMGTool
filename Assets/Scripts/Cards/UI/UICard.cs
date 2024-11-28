using MaloProduction.CustomAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace OMG.Card.UI
{
    public class UICard : MonoBehaviour
    {
        [Title("UI Card")]  
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

        [Header("Spells")]
        [SerializeField] private Transform spellsText;
        [SerializeField] private GameObject spellsLinePrefab;

        public virtual void Init(CardData cardData, CardOptions options)
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
            targetText.text = "Target : " + cardData.target.ToString();

            //Spells
            CreateSpellsText(cardData.spells);
        }

        private void CreateSpellsText(List<CardData.Spell> spellsBonus)
        {
            string spellsLineText = string.Empty;
            foreach (CardData.Spell spellBonus in spellsBonus)
            {
                spellsLineText += spellBonus.spellType.ToString(); // get the name of the spell by to string the enum
                spellsLineText += " : "; // separator
                spellsLineText += spellBonus.amount.ToString(); // value
                spellsLineText += "\n"; // line break

                GameObject spellsLine = Instantiate(spellsLinePrefab, spellsText);
                spellsLine.GetComponent<TextMeshProUGUI>().text = spellsLineText;
            }
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
}