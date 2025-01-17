namespace MaloProduction.Tween.DoTween.Module
{
    using MaloProduction.Tween.Core;
    using UnityEngine;
    using TMPro;

    public static class TextMeshProUGUIModule
    {
        public static TweenerCore<float, float> DoFade(this TextMeshProUGUI target, float endValue, float duration)
        {
            TweenerCore<float, float> tween = DOTween.To(
                () => target.color.a,
                (value) =>
                {
                    Color targetColor = target.color;
                    targetColor.a = value;
                    target.color = targetColor;
                },
                endValue, duration);

            tween.target = target;
            return tween;
        }

        public static TweenerCore<Color, Color> DoColor(this TextMeshProUGUI target, Color endValue, float duration)
        {
            TweenerCore<Color, Color> tween = DOTween.To(
                () => target.color,
                (value) =>
                {
                    target.color = value;
                },
                endValue, duration);

            tween.target = target;
            return tween;
        }
    }
}