namespace MVProduction.Tween.Plugin
{
    using MVProduction.Tween.Core;
    using MVProduction.Tween.Delegate;
    using MVProduction.Tween.Ease;
    using UnityEngine;

    public class Vector3Plugin : ABSPlugin<Vector3, Vector3>
    {
        public override void EvalutateAndApply(Tween tween, Vector3 startValue, Vector3 endValue, Vector3 changeValue, TweenSetter<Vector3> setter, TweenGetter<Vector3> getter, float elapsedTime, float duration)
        {
            float easeValue = EaseManager.Evaluate(tween.easeType, elapsedTime, duration);
            setter(startValue + changeValue * easeValue);
        }

        public override void SetChangeValue(TweenerCore<Vector3, Vector3> tween)
        {
            tween.changeValue = tween.endValue - tween.startValue;
        }

        public override void SetFrom(TweenerCore<Vector3, Vector3> tween)
        {
            tween.startValue = tween.getter();
        }
    }
}
