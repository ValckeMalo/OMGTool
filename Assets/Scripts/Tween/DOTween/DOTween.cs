namespace MaloProduction.Tween.DoTween
{
    using MaloProduction.Tween.Core;
    using MaloProduction.Tween.Plugin;
    using MaloProduction.Tween.Delegate;
    using UnityEngine;

    public static class DOTween
    {
        public static TweenerCore<T1, T2> To<T1, T2>(TweenGetter<T1> getter, TweenSetter<T1> setter, T2 endValue, float duration, ABSPlugin<T1, T2> plugin)
        {
            return ApplyTo(getter, setter, endValue, duration, plugin);
        }

        public static TweenerCore<Vector2, Vector2> To(TweenGetter<Vector2> getter, TweenSetter<Vector2> setter, Vector2 endValue, float duration)
        {
            return ApplyTo(getter, setter, endValue, duration);
        }

        private static TweenerCore<T1, T2> ApplyTo<T1, T2>(TweenGetter<T1> getter, TweenSetter<T1> setter, T2 endValue, float duration, ABSPlugin<T1, T2> plugin = null)
        {
            TweenerCore<T1, T2> tweener = TweenManager.GetTweener<T1, T2>();
            bool setupSuccessful = tweener.Setup(getter, setter, endValue, duration, plugin);

            if (!setupSuccessful)
            {
                TweenManager.Despawn(tweener);
                return null;
            }

            return tweener;
        }
    }
}