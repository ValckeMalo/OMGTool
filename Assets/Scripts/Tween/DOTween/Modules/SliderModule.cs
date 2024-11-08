namespace MaloProduction.Tween.DoTween.Module
{
    using MaloProduction.Tween.Core;
    using UnityEngine.UI;

    public static class SliderModule
    {
        public static TweenerCore<float, float> DoValue(this Slider target, float endValue, float duration)
        {
            TweenerCore<float, float> tween = DOTween.To(
                () => target.value,
                (value) =>
                {
                    target.value = value;
                },
                endValue, duration);

            tween.target = target;
            return tween;
        }
    }
}