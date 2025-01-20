namespace MVProduction.Tween.DoTween.Module
{
    using MVProduction.Tween.Core;
    using UnityEngine;
    using UnityEngine.UI;

    public static class ImageModule
    {
        public static TweenerCore<float, float> DoFade(this Image target, float endValue, float duration)
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

        public static TweenerCore<Color, Color> DoColor(this Image target, Color endValue, float duration)
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