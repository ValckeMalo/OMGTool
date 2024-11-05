namespace MaloProduction.Tween.Core
{
    using System.Collections.Generic;

    public static class TweenManager
    {
        private static List<Tween> activeTween = new List<Tween>();
        private static List<Tween> killTween = new List<Tween>();
        private static List<Tween> toAddTween = new List<Tween>();

        public static bool isUpdating = false;

        public static void Update(float deltaTime)
        {
            isUpdating = true;
            bool tweenToKill = false;
            foreach (Tween tween in activeTween)
            {
                if (UpdateTween(tween, deltaTime))
                {
                    tweenToKill = true;
                }
            }

            if (tweenToKill)
            {
                DespawnActiveTweens(killTween);
                killTween.Clear();
            }

            isUpdating = false;

            if (toAddTween.Count > 0)
            {
                foreach (Tween tween in toAddTween)
                {
                    AddActiveTween(tween);
                }
                toAddTween.Clear();
            }
        }

        private static void DespawnActiveTweens(List<Tween> killTween)
        {
            foreach (Tween tween in killTween)
            {
                Despawn(tween);
            }
        }

        /// <summary>
        /// return if the tween need to be killed
        /// </summary>
        /// <param name="tween"></param>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        private static bool UpdateTween(Tween tween, float deltaTime)
        {
            if (!tween.active)
            {
                MarkForKilling(tween);
                return true;
            }

            if (!tween.isDelayComplete)
            {
                UpdateTweenDelay(tween, deltaTime);
                return false;
            }

            if (!tween.isPlaying)
            {
                return false;
            }

            float tweenDeltaTime = deltaTime * tween.timeScale;
            if (!tween.startupDone)
            {
                if (!tween.Startup())
                {
                    MarkForKilling(tween);
                    return true;
                }
            }

            float tweenElapsedTime = tween.elapsedTime;

            if (tween.duration <= 0f)
            {
                tweenElapsedTime = 0f;
            }
            else
            {
                tweenElapsedTime += tweenDeltaTime;
            }

            bool needsKilling = Tween.DoGoto(tween, tweenElapsedTime);
            if (needsKilling)
            {
                MarkForKilling(tween);
                return true;
            }

            return false;
        }

        private static void UpdateTweenDelay(Tween tween, float deltaTime)
        {
            if (tween.delay < 0f)
            {
                tween.delay = 0f;
            }

            tween.delay += deltaTime;

            if (tween.delay >= tween.delayDuration)
            {
                tween.isDelayComplete = true;
            }
        }

        private static void MarkForKilling(Tween tween)
        {
            tween.active = false;
            killTween.Add(tween);
        }

        public static TweenerCore<T1, T2> GetTweener<T1, T2>()
        {
            TweenerCore<T1, T2> tween = new TweenerCore<T1, T2>();
            AddActiveTween(tween);
            return tween;
        }

        private static void AddActiveTween(Tween tween)
        {
            tween.active = true;
            tween.isPlaying = true;

            if (!isUpdating)
            {
                activeTween.Add(tween);
            }
            else
            {
                toAddTween.Add(tween);
            }
        }
        private static void RemoveActiveTween(Tween tween)
        {
            tween.active = false;
            activeTween.Remove(tween);
        }

        public static void Despawn(Tween tween)
        {
            if (tween.onKill != null)
            {
                Tween.OnTweenCallback(tween.onKill);
            }

            tween.active = false;
            RemoveActiveTween(tween);
            tween.Reset();
            tween = null;
        }

        public static void DespawnAll()
        {
            foreach (Tween tween in activeTween)
            {
                Despawn(tween);
            }
        }
    }
}