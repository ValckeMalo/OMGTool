namespace MaloProduction.Tween.Core
{
    using MaloProduction.Tween.Delegate;
    using MaloProduction.Tween.Ease.Easing;
    using System;

    public abstract class Tween
    {
        public Easing easeType = Easing.Linear;

        public TweenCallback onPlay = null;
        public TweenCallback onUpdate = null;
        public TweenCallback onPause = null;
        public TweenCallback onComplete = null;
        public TweenCallback onKill = null;

        public float duration = 0f;
        public float elapsedTime = 0f;
        public float timeScale = 1f;

        public object target = null;

        public bool startupDone = false;
        public bool playedOnce = false;
        public bool active = false;

        public bool isPlaying = false;
        public bool isComplete = false;

        public abstract bool AppplyTween();
        public abstract bool Startup();

        public virtual void Reset()
        {
            target = null;
            duration = 0f;
            elapsedTime = 0f;
            timeScale = 0f;

            onPlay = null;
            onUpdate = null;
            onPause = null;
            onComplete = null;
            onKill = null;
        }

        public static bool DoGoto(Tween tween, float toPosition)
        {
            if (!tween.startupDone)
            {
                if (!tween.Startup())
                {
                    return true;
                }
            }

            if (!tween.playedOnce)
            {
                tween.playedOnce = true;
                if (tween.onPlay != null)
                {
                    OnTweenCallback(tween.onPlay);
                    if (!tween.active)
                    {
                        return true;
                    }
                }
                if (tween.onPlay != null)
                {
                    OnTweenCallback(tween.onPlay);
                    if (!tween.active)
                    {
                        return true;
                    }
                }
            }

            float previousElapsedTime = tween.elapsedTime;
            bool wasComplete = tween.isComplete;

            if (tween.elapsedTime > tween.duration)
            {
                tween.elapsedTime = tween.duration;
            }
            else if (tween.elapsedTime <= 0f)
            {
                if (tween.isComplete)
                {
                    tween.elapsedTime = tween.duration;
                }
                else
                {
                    tween.elapsedTime = 0f;
                }
            }

            tween.elapsedTime = toPosition;
            bool wasPlaying = tween.isPlaying;
            if (tween.AppplyTween())
            {
                return true;
            }

            //CALL BACK
            if (tween.onUpdate != null)
            {
                OnTweenCallback(tween.onUpdate);
            }
            if (tween.isComplete && tween.onComplete != null)
            {
                OnTweenCallback(tween.onComplete);
            }
            if (!tween.isPlaying && wasPlaying && tween.onPause != null)
            {
                OnTweenCallback(tween.onPause);
            }

            return tween.isComplete;
        }


        #region Callback
        public static bool OnTweenCallback(TweenCallback callback)
        {
            callback();
            return true;
        }
        public static bool OnTweenCallback<T>(TweenCallback<T> callback, T param)
        {
            callback(param);
            return true;
        }
        #endregion
    }
}