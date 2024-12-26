namespace OMG.Battle.UI.Tooltip
{
    using MaloProduction.CustomAttributes;

    using OMG.Card.Data;

    using System.Collections;
    using System.Collections.Generic;

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
                instance = this;
                return;
            }

            Destroy(gameObject);
        }
        #endregion


        //TODO Rework
        [Title("Tooltip Manager")]
        [Header("Tooltip Settings")]
        [SerializeField] private List<Tooltip> tooltipsPool = new List<Tooltip>();
        private int indexTootip = 0;
        private float allSizeY = 0f;
        private const float marginTooltip = 5f;

        #region First Throw
        public void SpawnTooltipCard(CardData card, RectTransform cardTransform)
        {
            StartCoroutine(DelayedSpawnTooltipCard(card, cardTransform));
        }
        private IEnumerator DelayedSpawnTooltipCard(CardData card, RectTransform cardTransform)
        {
            yield return new WaitForSeconds(0.75f);

            if (tooltipsPool == null || tooltipsPool.Count <= 0) Debug.LogError("TooltipPool is equal to null");

            if (card.isEtheral)
            {
                DisplayText("Etheral :", "This card is destroyed from deck when Played.", cardTransform);
            }

            if (card.spells != null && card.spells.Length > 0)
            {
                Spell spell = card.spells[0];
                if (spell.initiative)
                {
                    DisplayText("Initiative :", "Trigger if the card is played first.", cardTransform);
                }

                DisplayText("Poison :", "Apply Poison.", cardTransform);
            }
        }
        void DisplayText(string header, string content, RectTransform cardTransform)
        {
            Tooltip current = tooltipsPool[indexTootip];
            current.Header.text = header;
            current.Content.text = content;

            current.Show();

            Vector3 position = cardTransform.position;
            position.x -= cardTransform.sizeDelta.x / 2f;
            position.y += cardTransform.sizeDelta.y / 2f - allSizeY;

            float sizeY = current.GoTo(position);

            allSizeY += sizeY + marginTooltip;
            indexTootip++;
        }

        public void HideTooltipCard()
        {
            foreach (Tooltip tooltip in tooltipsPool)
            {
                tooltip.Hide();
            }

            indexTootip = 0;
            allSizeY = 0f;
            StopAllCoroutines();
        }
        #endregion

        private void UpdateTootlipText(string header, string content)
        {
            tooltipsPool[0].Header.text = header;
            tooltipsPool[0].Content.text = content;
        }
        private void UpdateTootlipPos(Vector3 positionTooltip)
        {
            positionTooltip.x += marginTooltip;

            tooltipsPool[0].GoTo(positionTooltip);
        }

        public void ShowTooltip(string header, string content, Vector3 positionTooltip)
        {
            UpdateTootlipText(header, content);
            UpdateTootlipPos(positionTooltip);

            tooltipsPool[0].Show();
        }
    }
}