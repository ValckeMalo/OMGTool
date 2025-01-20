namespace MVProduction.Tween.Plugin
{
    using MVProduction.Tween.Delegate;
    using MVProduction.Tween.Ease;
    using MVProduction.Tween.Core;

    public class FloatPlugin : ABSPlugin<float, float>
    {
        public override void EvalutateAndApply(Tween tween, float startValue,
            float endValue, float changeValue,
            TweenSetter<float> setter, TweenGetter<float> getter,
            float elapsedTime, float duration)
        {
            setter(
                  startValue + changeValue * EaseManager.Evaluate(tween.easeType, elapsedTime, duration)
            );
        }

        public override void SetChangeValue(TweenerCore<float, float> tween)
        {
            tween.changeValue = tween.endValue - tween.startValue;
        }

        public override void SetFrom(TweenerCore<float, float> tween)
        {
            tween.startValue = tween.getter();
        }
    }
}