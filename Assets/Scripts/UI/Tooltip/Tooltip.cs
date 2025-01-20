namespace OMG.Battle.UI.Tooltip
{
    using MVProduction.CustomAttributes;
    using MVProduction.Tween.Core;
    using MVProduction.Tween.DoTween.Module;
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    using static OMG.Battle.UI.Tooltip.TooltipManager;

    public class Tooltip : MonoBehaviour
    {
        [Title("Tooltip")]
        [Header("Toolip Settings")]
        [SerializeField] private CanvasGroup tooltipGroup = null;

        [Header("Toolip Header")]
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI typeText;
        [SerializeField] private TextMeshProUGUI headerText;

        [Header("Toolip Body")]
        [SerializeField] private TextMeshProUGUI bodyText;

        private RectTransform rectTooltip = null;
        private TweenerCore<float, float> fadeTween = null;

        public void Init() => tooltipGroup.alpha = 0f;

        public void FadeTooltip(float endValue, float duration)
        {
            if (fadeTween != null)
                TweenManager.Despawn(fadeTween);

            tooltipGroup.DoFade(endValue, duration);
        }

        public void UpdateToNewData(TooltipData data)
        {
            if (data == null)
            {
                Debug.LogError("Data pass in the Tooltip is null");
                return;
            }

            if (data.TypeF == TooltipData.Type.CARD)
            {
                icon.enabled = false;
                typeText.enabled = false;
            }
            else
            {
                icon.enabled = true;
                typeText.enabled = true;

                typeText.text = data.TypeF.ToString();
                icon.sprite = data.Icon;
            }

            headerText.text = data.Header.ToUpper();
            bodyText.text = data.Body;
        }

        public void GoTo(Vector3 position)
        {
            if (rectTooltip == null)
                rectTooltip = GetComponent<RectTransform>();

            rectTooltip.position = position;
        }

        public float GetHeight()
        {
            return rectTooltip.sizeDelta.y;
        }

        public float GetWidht()
        {
            if (rectTooltip == null)
                rectTooltip = GetComponent<RectTransform>();

            return rectTooltip.sizeDelta.x;
        }
    }
}