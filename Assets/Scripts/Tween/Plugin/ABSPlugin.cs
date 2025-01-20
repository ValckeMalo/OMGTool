namespace MVProduction.Tween.Plugin
{
    using MVProduction.Tween.Delegate;
    using MVProduction.Tween.Core;

    public abstract class ABSPlugin<T1, T2> : IPlugin
    {
        public abstract void EvalutateAndApply(Tween tween, T1 startValue, T2 endValue, T2 changeValue, TweenSetter<T1> setter, TweenGetter<T1> getter, float elapsedTime, float duration);

        public abstract void SetChangeValue(TweenerCore<T1, T2> tween);

        public abstract void SetFrom(TweenerCore<T1, T2> tween);
    }
}