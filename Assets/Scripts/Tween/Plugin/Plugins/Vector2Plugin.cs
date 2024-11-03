namespace MaloProduction.Tween.Plugin
{
    using MaloProduction.Tween.Core;
    using MaloProduction.Tween.Delegate;
    using UnityEngine;

    public class Vector2Plugin : ABSPlugin<Vector2, Vector2>
    {
        public override void EvalutateAndApply(Tween tween, Vector2 startValue,
            Vector2 endValue, Vector2 changeValue,
            TweenSetter<Vector2> setter, TweenGetter<Vector2> getter,
            float elapsedTime, float duration)
        {
            setter(Vector2.Lerp(startValue, endValue, elapsedTime));
        }
    }
}