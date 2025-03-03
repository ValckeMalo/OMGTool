namespace OMG.Game.Tooltip
{
    using MVProduction.CustomAttributes;
    using MVProduction.Tween.Core;
    using MVProduction.Tween.DoTween.Module;

    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class TooltipUI : MonoBehaviour
    {
        [Title("Tooltip")]
        [SerializeField] private CanvasGroup tooltipGroup = null;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI typeText;
        [SerializeField] private TextMeshProUGUI headerText;
        [SerializeField] private TextMeshProUGUI bodyText;

        //Tween
        private const float tooltipFade = 0.1f;
        private TweenerCore<float, float> fadeTween = null;

        public void Hide()
        {
            if (fadeTween != null)
                TweenManager.Despawn(fadeTween);

            tooltipGroup.alpha = 0f;
        }

        public void Show()
        {
            if (fadeTween != null)
                TweenManager.Despawn(fadeTween);

            fadeTween = tooltipGroup.DoFade(1f, tooltipFade);
        }

        public void SetTooltipData(TooltipData data)
        {
            if (data == null)
                return;

            if (data.Type == TooltipType.CARD)
            {
                icon.enabled = false;
                typeText.enabled = false;
            }
            else
            {
                icon.enabled = true;
                typeText.enabled = true;

                typeText.text = data.Type.ToString();
                icon.sprite = data.Icon;
            }

            headerText.text = data.Header.ToUpper();
            bodyText.text = data.Body;
        }
    }
}