namespace MVProduction.Tween.Plugin
{
    using MVProduction.Tween.Core;
    using MVProduction.Tween.Delegate;
    using MVProduction.Tween.Ease;
    using UnityEngine;

    public class QuaternionPlugin : ABSPlugin<Quaternion, Quaternion>
    {
        public override void EvalutateAndApply(Tween tween, Quaternion startValue, Quaternion endValue, Quaternion changeValue, TweenSetter<Quaternion> setter, TweenGetter<Quaternion> getter, float elapsedTime, float duration)
        {
            float easeValue = EaseManager.Evaluate(tween.easeType, elapsedTime, duration);

            Vector3 startEuler = startValue.eulerAngles;
            Vector3 changeEuler = changeValue.eulerAngles;
            Vector3 evaluateEuler = startEuler + changeEuler * easeValue;

            setter(Quaternion.Euler(evaluateEuler));
        }

        public override void SetChangeValue(TweenerCore<Quaternion, Quaternion> tween)
        {
            Vector3 startEuler = tween.startValue.eulerAngles;
            Vector3 endEuler = tween.endValue.eulerAngles;
            Vector3 changeValueEuler = endEuler - startEuler;

            tween.changeValue = Quaternion.Euler(changeValueEuler);
        }

        public override void SetFrom(TweenerCore<Quaternion, Quaternion> tween)
        {
            tween.startValue = tween.getter();
        }
    }
}