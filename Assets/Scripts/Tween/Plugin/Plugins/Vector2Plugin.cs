namespace MVProduction.Tween.Plugin
{
    using MVProduction.Tween.Core;
    using MVProduction.Tween.Delegate;
    using MVProduction.Tween.Ease;
    using UnityEngine;

    public class Vector2Plugin : ABSPlugin<Vector2, Vector2>
    {
        public override void EvalutateAndApply(Tween tween, Vector2 startValue,
            Vector2 endValue, Vector2 changeValue,
            TweenSetter<Vector2> setter, TweenGetter<Vector2> getter,
            float elapsedTime, float duration)
        {
            float easeValue = EaseManager.Evaluate(tween.easeType, elapsedTime, duration);
            setter(startValue + changeValue * easeValue);
        }

        public override void SetChangeValue(TweenerCore<Vector2, Vector2> tween)
        {
            tween.changeValue = tween.endValue - tween.startValue;
        }

        public override void SetFrom(TweenerCore<Vector2, Vector2> tween)
        {
            tween.startValue = tween.getter();
        }
    }
}