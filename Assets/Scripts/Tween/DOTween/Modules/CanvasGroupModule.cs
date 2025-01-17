namespace MaloProduction.Tween.DoTween.Module
{
    using MaloProduction.Tween.Core;
    using UnityEngine;

    public static class CanvasGroupModule
    {
        public static TweenerCore<float, float> DoFade(this CanvasGroup target, float endValue, float duration)
        {
            TweenerCore<float, float> tween = DOTween.To(
                () => target.alpha,
                (value) =>
                {
                    target.alpha = value;
                },
                endValue, duration);

            tween.target = target;
            return tween;
        }
    }
}