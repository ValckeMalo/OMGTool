namespace OMG.Battle.UI
{
    using MaloProduction.CustomAttributes;
    using OMG.Card.Data;
    using System.Collections;
    using TMPro;
    using UnityEngine;

    public class TooltipManager : MonoBehaviour
    {
        #region Singleton
        private static TooltipManager instance;
        public static TooltipManager Instance { get { return instance; } }

        private void Awake()
        {
            if (instance == null)
            {
                FillTooltipPool();
                instance = this;
                return;
            }

            Destroy(gameObject);
        }
        #endregion

        private struct Tooltip
        {
            public RectTransform rectTransform;
            public TMP_Text textComponent;

            public Tooltip(RectTransform transform, TMP_Text text)
            {
                this.rectTransform = transform;
                this.textComponent = text;
            }
        }


        [Title("Tooltip Manager")]
        [Header("Tooltip Settings")]
        [SerializeField] private GameObject prefabTooltip;
        [SerializeField] private RectTransform parent;
        private Tooltip[] tooltipsPool = new Tooltip[5];

        private void FillTooltipPool()
        {
            for (int i = 0; i < tooltipsPool.Length; i++)
            {
                GameObject newTooltip = Instantiate(prefabTooltip, parent);
                newTooltip.SetActive(false);

                tooltipsPool[i] = new Tooltip(newTooltip.GetComponent<RectTransform>(), newTooltip.GetComponentInChildren<TMP_Text>());
            }
        }
        public void SpawnTooltipCard(CardData card, RectTransform cardTransform)
        {
            StartCoroutine(DelayedSpawnTooltipCard(card, cardTransform));
        }
        private IEnumerator DelayedSpawnTooltipCard(CardData card, RectTransform cardTransform)
        {
            yield return new WaitForSeconds(0.75f);

            const int fontSizeTitle = 20;
            const int fontSizeText = 18;
            int indexTooltipPool = 0;
            float allSize = 0f;

            void Display(string text)
            {
                Tooltip current = tooltipsPool[indexTooltipPool];
                TMP_Text currentTextComponent = current.textComponent;
                RectTransform rectTooltip = current.rectTransform;

                rectTooltip.gameObject.SetActive(true);

                currentTextComponent.text = text;
                Vector2 sizeDeleta = new Vector2(currentTextComponent.preferredWidth, currentTextComponent.preferredHeight);
                rectTooltip.sizeDelta = sizeDeleta;
                print(sizeDeleta);

                Vector2 cardPos = cardTransform.position;
                Vector2 cardSize = cardTransform.sizeDelta;

                Vector2 tooltipPos = cardPos;
                tooltipPos.x -= (cardSize.x / 2f);
                tooltipPos.y += (cardSize.y / 2f) + allSize * 2f;
                rectTooltip.position = tooltipPos;

                allSize += sizeDeleta.y;
                indexTooltipPool++;

            }

            if (card.isEtheral)
            {
                Display($"<size={fontSizeTitle}><b>Etheral :</b></size>\n<size={fontSizeText}>This card is destroyed from deck when Played.</size>");
                //Tooltip current = tooltipsPool[indexTooltipPool];
                //TMP_Text currentTextComponent = current.textComponent;
                //RectTransform rectTooltip = current.rectTransform;

                //rectTooltip.gameObject.SetActive(true);

                //currentTextComponent.text = ;
                //Vector2 sizeDeleta = new Vector2(currentTextComponent.preferredWidth, currentTextComponent.preferredHeight);
                //rectTooltip.sizeDelta = sizeDeleta;
                //print(sizeDeleta);

                //Vector2 cardPos = cardTransform.position;
                //Vector2 cardSize = cardTransform.sizeDelta;

                //Vector2 tooltipPos = cardPos;
                //tooltipPos.x -= (cardSize.x / 2f) ;
                //tooltipPos.y += (cardSize.y / 2f) ;
                //rectTooltip.position = tooltipPos;

                //indexTooltipPool++;
            }

            if (card.spells != null && card.spells.Length > 0)
            {
                Spell spell = card.spells[0];

                //Tooltip current;
                //TMP_Text currentTextComponent;
                //RectTransform rectTooltip;

                if (spell.initiative)
                {
                    Display($"<size={fontSizeTitle}><b>Initiative :</b></size>\n<size={fontSizeText}>Trigger if the card is played first.</size>");

                    //current = tooltipsPool[indexTooltipPool];
                    //currentTextComponent = current.textComponent;
                    //rectTooltip = current.rectTransform;

                    //currentTextComponent.text = "<size=35><b>Initiative : </b></size> \n<size=20>Trigger if the card is played first.</size>";

                    //current.rectTransform.sizeDelta = new Vector2(currentTextComponent.preferredWidth, currentTextComponent.preferredHeight);
                    //current.rectTransform.gameObject.SetActive(true);

                    //rectTooltip.anchoredPosition = cardTransform.anchoredPosition;
                    //indexTooltipPool++;
                }

                Display($"<size={fontSizeTitle}><b>Poison :</b></size>\n<size={fontSizeText}>Apply Poison.</size>");

                //current = tooltipsPool[indexTooltipPool];
                //currentTextComponent = current.textComponent;
                //rectTooltip = current.rectTransform;

                //currentTextComponent.text = "<size=35><b>Poison : </b></size> \nThis card is destroyed from deck when Played";

                //current.rectTransform.sizeDelta = new Vector2(currentTextComponent.preferredWidth, currentTextComponent.preferredHeight);
                //current.rectTransform.gameObject.SetActive(true);
                //rectTooltip.anchoredPosition = cardTransform.anchoredPosition;
                //indexTooltipPool++;
            }
        }

        public void HideTooltipCard()
        {
            foreach (Tooltip tooltip in tooltipsPool)
            {
                tooltip.rectTransform.gameObject.SetActive(false);
            }

            StopAllCoroutines();
        }
    }
}