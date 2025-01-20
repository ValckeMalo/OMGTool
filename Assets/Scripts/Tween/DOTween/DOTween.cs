namespace MVProduction.Tween.DoTween
{
    using MVProduction.Tween.Core;
    using MVProduction.Tween.Plugin;
    using MVProduction.Tween.Delegate;
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

        public static TweenerCore<float, float> To(TweenGetter<float> getter, TweenSetter<float> setter, float endValue, float duration)
        {
            return ApplyTo(getter, setter, endValue, duration);
        }

        public static TweenerCore<Color, Color> To(TweenGetter<Color> getter, TweenSetter<Color> setter, Color endValue, float duration)
        {
            return ApplyTo(getter, setter, endValue, duration);
        }

        public static TweenerCore<Vector3, Vector3> To(TweenGetter<Vector3> getter, TweenSetter<Vector3> setter, Vector3 endValue, float duration)
        {
            return ApplyTo(getter, setter, endValue, duration);
        }

        public static TweenerCore<Quaternion, Quaternion> To(TweenGetter<Quaternion> getter, TweenSetter<Quaternion> setter, Quaternion endValue, float duration)
        {
            return ApplyTo(getter, setter, endValue, duration);
        }

        private static TweenerCore<T1, T2> ApplyTo<T1, T2>(TweenGetter<T1> getter, TweenSetter<T1> setter, T2 endValue, float duration, ABSPlugin<T1, T2> plugin = null)
        {
            TweenerCore<T1, T2> tweener = TweenManager.GetTweener<T1, T2>();
            bool setupSuccessful = tweener.Setup(getter, setter, endValue,duration, plugin);

            if (!setupSuccessful)
            {
                TweenManager.Despawn(tweener);
                return null;
            }

            return tweener;
        }
    }
}