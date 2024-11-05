namespace MaloProduction.Tween
{
    using MaloProduction.Tween.Core;
    using MaloProduction.Tween.Delegate;

    public static class TweenExtension
    {
        public static TweenerCore<T1, T2> OnPlay<T1, T2>(this TweenerCore<T1, T2> tweener, TweenCallback onPlay)
        {
            tweener.onPlay = onPlay;
            return tweener;
        }
        public static TweenerCore<T1, T2> OnUpdate<T1, T2>(this TweenerCore<T1, T2> tweener, TweenCallback onUpdate)
        {
            tweener.onUpdate = onUpdate;
            return tweener;
        }
        public static TweenerCore<T1, T2> OnComplete<T1, T2>(this TweenerCore<T1, T2> tweener, TweenCallback onComplete)
        {
            tweener.onComplete = onComplete;
            return tweener;
        }

        public static TweenerCore<T1, T2> AddDelay<T1, T2>(this TweenerCore<T1, T2> tweener, float delay)
        {
            if (delay < 0f)
            {
                tweener.delayDuration = 0f;
                tweener.isDelayComplete = true;

                return tweener;
            }

            tweener.delayDuration = delay;
            tweener.isDelayComplete = false;
            tweener.delay = 0f;

            return tweener;
        }
    }
}