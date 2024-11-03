namespace MaloProduction.Tween.DoTween.Module
{
    using MaloProduction.Tween.Core;
    using MaloProduction.Tween.Delegate;
    using UnityEngine;

    public static class Vector2Module
    {
        public static TweenerCore<Vector2, Vector2> DoMove(this Vector2 target, Vector2 endValue, float duration, TweenGetter<Vector2> getter,TweenSetter<Vector2> setter)
        {
            TweenerCore<Vector2, Vector2> tween = DOTween.To(getter, setter, endValue, duration);
            tween.target = target;
            return tween;
        }
    }
}