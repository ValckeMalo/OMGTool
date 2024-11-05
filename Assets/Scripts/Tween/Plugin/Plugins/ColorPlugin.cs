namespace MaloProduction.Tween.Plugin
{
    using MaloProduction.Tween.Core;
    using MaloProduction.Tween.Delegate;
    using MaloProduction.Tween.Ease;
    using UnityEngine;

    public class ColorPlugin : ABSPlugin<Color, Color>
    {
        public override void EvalutateAndApply(Tween tween, Color startValue, Color endValue, Color changeValue, TweenSetter<Color> setter, TweenGetter<Color> getter, float elapsedTime, float duration)
        {
            float easeValue = EaseManager.Evaluate(tween.easeType, elapsedTime, duration);
            setter(startValue + changeValue * easeValue);
        }

        public override void SetChangeValue(TweenerCore<Color, Color> tween)
        {
            tween.changeValue = tween.endValue - tween.startValue;
        }

        public override void SetFrom(TweenerCore<Color, Color> tween)
        {
            tween.startValue = tween.getter();
        }
    }
}