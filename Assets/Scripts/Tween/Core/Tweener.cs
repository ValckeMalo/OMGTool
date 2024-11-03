namespace MaloProduction.Tween.Core
{
    public abstract class Tweener : Tween
    {
        public static bool DoStartup<T1,T2>(TweenerCore<T1,T2> tween)
        {
            tween.startupDone = true;
            tween.active = true;

            return true;
        }
    }
}