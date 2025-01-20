namespace OMG.Card.UI
{
    using MVProduction.CustomAttributes;

    using OMG.Card.Data;

    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

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

        protected CardData cardData;

        public virtual void Init(CardData cardData, CardOptions options)
        {
            this.cardData = cardData;

            background.sprite = options.backgroundSprite[(int)cardData.background].sprite;

            //Header Section
            iconValue.sprite = options.iconSprite[(int)cardData.type].sprite;
            valueText.text = cardData.value.ToString();

            wakfu.sprite = options.wakfu;
            wakfuCostText.text = cardData.wakfu.ToString();

            //Center Section
            icon.sprite = cardData.icon;

            //Foot Section
            nameText.text = cardData.name;

            //Target
            targetText.text = "Target : " + cardData.target.ToString();

            //Spells
            CreateSpellsText(cardData.spells);
        }

        private void CreateSpellsText(Spell[] spellsBonus)
        {
            string spellsLineText = string.Empty;
            foreach (Spell spellBonus in spellsBonus)
            {
                spellsLineText += spellBonus.type.ToString(); // get the name of the spell by to string the enum
                spellsLineText += " : "; // separator
                spellsLineText += spellBonus.value.ToString(); // value
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

        protected void UpdateUI(int wakfuValue, int cardValue)
        {
            valueText.text = cardValue.ToString();
            wakfuCostText.text = wakfuValue.ToString();

            if (cardData.value != cardValue)
                valueText.color = Color.green;
            if (cardData.wakfu != wakfuValue)
                wakfuCostText.color = Color.green;
        }
    }
}