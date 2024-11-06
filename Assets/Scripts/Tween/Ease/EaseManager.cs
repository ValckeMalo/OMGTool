namespace MaloProduction.Tween.Ease
{
    using UnityEngine;

    public static class EaseManager
    {
        const float c1 = 1.70158f;
        const float c2 = c1 * 1.525f;
        const float c3 = c1 + 1f;
        const float c4 = (2f * Mathf.PI) / 3f;
        const float c5 = (2f * Mathf.PI) / 4.5f;

        const float n1 = 7.5625f;
        const float d1 = 2.75f;

        public static float Evaluate(Easing easing, float time, float duration)
        {
            float x = time / duration;

            switch (easing)
            {
                case Easing.Linear:
                    return x;

                case Easing.InSine:
                    return 1f - Mathf.Cos((x * Mathf.PI) / 2f);

                case Easing.OutSine:
                    return Mathf.Sin((x * Mathf.PI) / 2f);

                case Easing.InOutSine:
                    return -(Mathf.Cos(Mathf.PI * x) - 1f) / 2f;

                case Easing.InCubic:
                    return x * x * x;

                case Easing.OutCubic:
                    return 1f - Mathf.Pow(1f - x, 3f);

                case Easing.InOutCubic:
                    return x < 0.5f ?
                        4f * x * x * x :
                        1f - Mathf.Pow(-2f * x + 2f, 3f) / 2f;

                case Easing.InQuint:
                    return x * x * x * x * x;

                case Easing.OutQuint:
                    return 1f - Mathf.Pow(1f - x, 5f);

                case Easing.InOutQuint:
                    return x < 0.5f ?
                        16f * x * x * x * x * x :
                        1f - Mathf.Pow(-2f * x + 2f, 5f) / 2f;

                case Easing.InCirc:
                    return 1f - Mathf.Sqrt(1f - Mathf.Pow(x, 2f));

                case Easing.OutCirc:
                    return Mathf.Sqrt(1f - Mathf.Pow(x - 1f, 2f));

                case Easing.InOutCirc:
                    return x < 0.5f
                      ? (1f - Mathf.Sqrt(1f - Mathf.Pow(2f * x, 2f))) / 2f
                      : (Mathf.Sqrt(1f - Mathf.Pow(-2f * x + 2f, 2f)) + 1f) / 2f;

                case Easing.InElastic:
                    return x == 0f ?
                        0f :
                        x == 1f ?
                            1f :
                            -Mathf.Pow(2f, 10f * x - 10f) * Mathf.Sin((x * 10f - 10.75f) * c4);

                case Easing.OutElastic:
                    return x == 0f
                      ? 0f
                      : x == 1f
                        ? 1f
                        : Mathf.Pow(2f, -10f * x) * Mathf.Sin((x * 10f - 0.75f) * c4) + 1f;

                case Easing.InOutElastic:

                    return x == 0f
                      ? 0f
                      : x == 1f
                        ? 1f
                        : x < 0.5f
                            ? -(Mathf.Pow(2f, 20f * x - 10f) * Mathf.Sin((20f * x - 11.125f) * c5)) / 2f
                            : (Mathf.Pow(2f, -20f * x + 10f) * Mathf.Sin((20f * x - 11.125f) * c5)) / 2f + 1f;

                case Easing.InQuad:
                    return x * x;

                case Easing.OutQuad:
                    return 1f - (1f - x) * (1f - x);

                case Easing.InOutQuad:
                    return x < 0.5f ?
                        2f * x * x :
                        1f - Mathf.Pow(-2f * x + 2f, 2f) / 2f;

                case Easing.InQuart:
                    return x * x * x * x;

                case Easing.OutQuart:
                    return 1f - Mathf.Pow(1f - x, 4f);

                case Easing.InOutQuart:
                    return x < 0.5f ?
                        8f * x * x * x * x :
                        1f - Mathf.Pow(-2f * x + 2f, 4f) / 2f;

                case Easing.InExpo:
                    return x == 0f ?
                        0f :
                        Mathf.Pow(2f, 10f * x - 10f);

                case Easing.OutExpo:
                    return x == 1f ?
                        1f :
                        1f - Mathf.Pow(2f, -10f * x);

                case Easing.InOutExpo:
                    return x == 0f
                      ? 0f
                      : x == 1f
                        ? 1f
                        : x < 0.5f ?
                            Mathf.Pow(2f, 20f * x - 10f) / 2f :
                            (2f - Mathf.Pow(2f, -20f * x + 10f)) / 2f;

                case Easing.InBack:
                    return c3 * x * x * x - c1 * x * x;

                case Easing.OutBack:
                    return 1f + c3 * Mathf.Pow(x - 1f, 3f) + c1 * Mathf.Pow(x - 1f, 2f);

                case Easing.InOutBack:
                    return x < 0.5f
                      ? (Mathf.Pow(2f * x, 2f) * ((c2 + 1f) * 2f * x - c2)) / 2f
                      : (Mathf.Pow(2f * x - 2f, 2f) * ((c2 + 1f) * (x * 2f - 2f) + c2) + 2f) / 2f;

                case Easing.InBounce:
                    return 1f - EaseOutBounce(1f - x);

                case Easing.OutBounce:
                    return EaseOutBounce(x);

                case Easing.InOutBounce:
                    return x < 0.5f
                      ? (1f - EaseOutBounce(1f - 2f * x)) / 2f
                      : (1f + EaseOutBounce(2f * x - 1f)) / 2f;

                default:
                    return x;
            }
        }

        private static float EaseOutBounce(float x)
        {
            if (x < 1f / d1)
            {
                return n1 * x * x;
            }
            else if (x < 2f / d1)
            {
                return n1 * (x -= 1.5f / d1) * x + 0.75f;
            }
            else if (x < 2.5f / d1)
            {
                return n1 * (x -= 2.25f / d1) * x + 0.9375f;
            }
            else
            {
                return n1 * (x -= 2.625f / d1) * x + 0.984375f;
            }
        }
    }
}