namespace MVProduction.Tween.DoTween.Module
{
    using MVProduction.Tween.Core;
    using UnityEngine;

    public static class TransformModule
    {
        public static TweenerCore<Vector3, Vector3> DoMove(this Transform target, Vector3 endValue, float duration)
        {
            return target.position.DoMove(endValue,duration,
                () => target.position,

                (newPos) =>
                {
                    target.position = newPos;
                });
        }

        //public static TweenerCore<Vector3, Vector3> DoScale(this Transform target, Vector3 endValue, float duration)
        //{

        //}

        //public static TweenerCore<Quaternion, Quaternion> Do2DRotation(this Transform target, Vector2 endValue, float duration)
        //{

        //}

        //public static TweenerCore<Quaternion, Quaternion> Do3DRotation(this Transform target, Vector3 endValue, float duration)
        //{

        //}
    }
}