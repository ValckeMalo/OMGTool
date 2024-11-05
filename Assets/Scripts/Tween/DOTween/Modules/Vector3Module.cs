namespace MaloProduction.Tween.DoTween.Module
{
    using MaloProduction.Tween.Core;
    using MaloProduction.Tween.Delegate;
    using UnityEngine;

    public static class Vector3Module
    {
        public static TweenerCore<Vector3, Vector3> DoMove(this Vector3 target, Vector3 endValue, float duration, TweenGetter<Vector3> getter, TweenSetter<Vector3> setter)
        {
            TweenerCore<Vector3, Vector3> tween = DOTween.To(getter, setter, endValue, duration);
            tween.target = target;
            return tween;
        }
    }
}