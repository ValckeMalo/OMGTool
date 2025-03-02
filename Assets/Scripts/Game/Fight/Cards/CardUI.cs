namespace OMG.Game.Fight.Cards
{
    using MVProduction.CustomAttributes;
    
    using OMG.Data.Card;
    
    using TMPro;

    using UnityEngine;
    using UnityEngine.UI;

    public class CardUI : MonoBehaviour
    {
        [Title("Card UI")]
        [SerializeField] protected RectTransform cardRect;

        [Header("Infos")]
        [SerializeField] protected Image background;
        [SerializeField] protected Image icon;

        [Header("Value")]
        [SerializeField] protected Image valueIcon;
        [SerializeField] protected TextMeshProUGUI valueText;

        [Header("Energy")]
        [SerializeField] protected Image energyIcon;
        [SerializeField] protected TextMeshProUGUI energyText;

        [Header("Description")]
        [SerializeField] protected TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI targetText;

        [Header("Spell")]
        [SerializeField] protected Transform containerSpell;
        [SerializeField] protected GameObject spellLinePrefab;

        protected CardData cardData = null;
        protected CardSettings settings = null;

        public void SetData(CardData cardData, CardSettings settings)
        {
            this.cardData = cardData;
            this.settings = settings;
        }

        protected void UpdateCardUI()
        {
            //Infos
            if (background != null)
                background.sprite = settings.backgroundSprite[(int)cardData.background].sprite;

            if (icon != null)
                icon.sprite = cardData.icon;

            //Value
            if (valueIcon != null)
                valueIcon.sprite = settings.iconSprite[(int)cardData.type].sprite;

            if (valueText != null)
                valueText.text = cardData.value.ToString();

            //Energy
            if (energyIcon != null)
                energyIcon.sprite = settings.energy;

            if (energyText != null)
                energyText.text = cardData.energy.ToString();

            //Description
            if (titleText != null)
                titleText.text = cardData.name;

            if (targetText != null)
                targetText.text = "Target : " + cardData.target.ToString();

            //Spell
            if (containerSpell != null && spellLinePrefab != null)
            {
                ClearSpellsLine();
                UpdateSpellsLine();
            }
        }

        protected void ClearSpellsLine()
        {
            //Destroy all previous spell line
            for (int i = containerSpell.childCount - 1; i >= 0; i--)
            {
                Destroy(containerSpell.GetChild(i).gameObject);
            }
        }

        protected void UpdateSpellsLine()
        {
            //create all the spell line
            string spellLineText = string.Empty;
            foreach (CardSpell spell in cardData.spells)
            {
                spellLineText += spell.type.ToString(); // get the name of the spell by to string the enum
                spellLineText += " : "; // separator
                spellLineText += spell.value.ToString(); // value
                spellLineText += "\n"; // line break

                GameObject spellLine = Instantiate(spellLinePrefab, containerSpell);
                spellLine.GetComponent<TextMeshProUGUI>().text = spellLineText;
            }
        }
    }
}