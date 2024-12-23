namespace OMG.Battle.UI.Tooltip
{
    using MaloProduction.CustomAttributes;
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    [ExecuteInEditMode]
    public class Tooltip : MonoBehaviour
    {
        [Title("Tooltip")]
        [Header("Toolip Stettings")]
        [SerializeField] private TextMeshProUGUI headerField;
        [SerializeField] private TextMeshProUGUI contentField;
        [SerializeField] private LayoutElement layoutElement;
        private RectTransform rectTooltip = null;

        public TextMeshProUGUI Content => contentField;
        public TextMeshProUGUI Header => headerField;

        public void Hide() => gameObject.SetActive(false);

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public float GoTo(Vector3 position)
        {
            if (rectTooltip == null)
                rectTooltip = GetComponent<RectTransform>();
            rectTooltip.position = position;

            return rectTooltip.sizeDelta.y;
        }
    }
}