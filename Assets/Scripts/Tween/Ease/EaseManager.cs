namespace MaloProduction.Tween.Ease
{
    using MaloProduction.Tween.Ease.Easing;

    public static class EaseManager
    {
        public static float Evaluate(Easing.Easing easeType, float time, float duration)
        {
            switch (easeType)
            {
                case Easing.Easing.Linear:
                    return time / duration;

                default:
                    return time / duration;
            }
        }
    }
}

namespace MaloProduction.Tween.Ease.Easing
{
    public enum Easing
    {
        Linear,
    }
}