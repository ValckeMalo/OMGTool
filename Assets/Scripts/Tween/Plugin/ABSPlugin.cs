namespace MaloProduction.Tween.Plugin
{
    using MaloProduction.Tween.Delegate;
    using MaloProduction.Tween.Core;
    
    public abstract class ABSPlugin<T1, T2> : IPlugin
    {
        public abstract void EvalutateAndApply(Tween tween, T1 startValue, T2 endValue, T2 changeValue, TweenSetter<T1> setter, TweenGetter<T1> getter, float elapsedTime, float duration);
    }
}