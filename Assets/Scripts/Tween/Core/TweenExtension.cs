namespace MVProduction.Tween
{
    using MVProduction.Tween.Core;
    using MVProduction.Tween.Delegate;
    using MVProduction.Tween.Ease;

    public static class TweenExtension
    {
        #region Callback
        /// <summary>
        /// Add a callback when tween start
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="tweener"></param>
        /// <param name="onStart"></param>
        /// <returns></returns>
        public static TweenerCore<T1, T2> OnStart<T1, T2>(this TweenerCore<T1, T2> tweener, TweenCallback onStart)
        {
            tweener.onStart = onStart;
            return tweener;
        }
        /// <summary>
        /// Add a callback when tween Play if it was paused before
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="tweener"></param>
        /// <param name="onStart"></param>
        /// <returns></returns>
        public static TweenerCore<T1, T2> OnPlay<T1, T2>(this TweenerCore<T1, T2> tweener, TweenCallback onPlay)
        {
            tweener.onPlay = onPlay;
            return tweener;
        }
        /// <summary>
        /// Add a callback when tween update
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="tweener"></param>
        /// <param name="onStart"></param>
        /// <returns></returns>
        public static TweenerCore<T1, T2> OnUpdate<T1, T2>(this TweenerCore<T1, T2> tweener, TweenCallback onUpdate)
        {
            tweener.onUpdate = onUpdate;
            return tweener;
        }
        /// <summary>
        /// Add a callback when tween is complete
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="tweener"></param>
        /// <param name="onStart"></param>
        /// <returns></returns>
        public static TweenerCore<T1, T2> OnComplete<T1, T2>(this TweenerCore<T1, T2> tweener, TweenCallback onComplete)
        {
            tweener.onComplete = onComplete;
            return tweener;
        }
        /// <summary>
        /// Add a callback when tween is paused
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="tweener"></param>
        /// <param name="onStart"></param>
        /// <returns></returns>
        public static TweenerCore<T1, T2> OnPause<T1, T2>(this TweenerCore<T1, T2> tweener, TweenCallback onPause)
        {
            tweener.onPause = onPause;
            return tweener;
        }
        /// <summary>
        /// Add a callback when tween is Destroyed/Kill
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="tweener"></param>
        /// <param name="onStart"></param>
        /// <returns></returns>
        public static TweenerCore<T1, T2> OnKill<T1, T2>(this TweenerCore<T1, T2> tweener, TweenCallback onKill)
        {
            tweener.onKill = onKill;
            return tweener;
        }
        #endregion

        /// <summary>
        /// Add a delay to the tween, to start it later
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="tweener"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public static TweenerCore<T1, T2> AddDelay<T1, T2>(this TweenerCore<T1, T2> tweener, float delay)
        {
            if (delay <= 0f)
            {
                tweener.delayDuration = 0f;
                tweener.isDelayComplete = true;
                tweener.delay = 0f;

                return tweener;
            }

            tweener.delayDuration = delay;
            tweener.isDelayComplete = false;
            tweener.isPlaying = false; //to don't update the tween when we have a delay
            tweener.delay = 0f;

            return tweener;
        }

        /// <summary>
        /// Change the ease of the tween
        /// Base Ease = Linear 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="tweener"></param>
        /// <param name="ease"></param>
        /// <returns></returns>
        public static TweenerCore<T1, T2> SetEase<T1, T2>(this TweenerCore<T1, T2> tweener, Easing ease)
        {
            tweener.easeType = ease;
            return tweener;
        }

        /// <summary>
        /// Change the repeat mode of the tween
        /// Base RepeatMode = Once
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="tweener"></param>
        /// <param name="repeatMode"></param>
        /// <returns></returns>
        public static TweenerCore<T1,T2> SetReapeatMode<T1, T2>(this TweenerCore<T1, T2> tweener, TweenRepeatMode repeatMode)
        {
            tweener.repeatMode = repeatMode;
            return tweener;
        }
    }
}