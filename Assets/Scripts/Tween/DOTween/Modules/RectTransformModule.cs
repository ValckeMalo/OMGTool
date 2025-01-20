namespace MVProduction.Tween.DoTween.Module
{
    using MVProduction.Tween.Core;
    using UnityEngine;

    public static class RectTransformModule
    {
        public static TweenerCore<Vector2, Vector2> DoMove(this RectTransform target, Vector2 endValue, float duration)
        {
            TweenerCore<Vector2, Vector2> tween = DOTween.To(
                () => new Vector2(target.position.x, target.position.y),//setter
                (value) => //getter
                {
                    Vector3 newPos = new Vector3(value.x, value.y, target.position.z);
                    target.position = newPos;
                },
            endValue,  //final value
            duration);// duration

            tween.target = target;
            return tween;
        }
        public static TweenerCore<Vector2, Vector2> DoAnchorMove(this RectTransform target, Vector2 endValue, float duration)
        {
            TweenerCore<Vector2, Vector2> tween = DOTween.To(
                () => target.anchoredPosition,
                (value) =>
                {
                    target.anchoredPosition = value;
                },
            endValue, duration);

            tween.target = target;
            return tween;
        }

        public static TweenerCore<Vector3, Vector3> DoMove(this RectTransform target, Vector3 endValue, float duration)
        {
            TweenerCore<Vector3, Vector3> tween = DOTween.To(
                () => target.position,
                (value) =>
                {
                    target.position = value;
                },
            endValue, duration);

            tween.target = target;
            return tween;
        }

        public static TweenerCore<Vector2, Vector2> DoScale(this RectTransform target, Vector2 endValue, float duration)
        {
            TweenerCore<Vector2, Vector2> tween = DOTween.To(
                () => target.sizeDelta,
                (value) =>
                {
                    target.sizeDelta = value;
                },
            endValue, duration);

            tween.target = target;
            return tween;
        }

        public static TweenerCore<Quaternion, Quaternion> Do2DRotation(this RectTransform target, Vector2 endValueEuler, float duration)
        {
            Vector3 eulerRotation = target.rotation.eulerAngles;
            Quaternion endQuaternion = Quaternion.Euler(new Vector3(eulerRotation.x, endValueEuler.x, endValueEuler.y));
            TweenerCore<Quaternion, Quaternion> tween = DOTween.To(
                () => target.rotation,
                (value) =>
                {
                    target.rotation = value;
                },
            endQuaternion, duration);

            tween.target = target;
            return tween;
        }

        public static TweenerCore<Quaternion, Quaternion> Do3DRotation(this RectTransform target, Vector3 endValueEuler, float duration)
        {
            Quaternion endQuaternion = Quaternion.Euler(endValueEuler);
            TweenerCore<Quaternion, Quaternion> tween = DOTween.To(
                () => target.rotation,
                (value) =>
                {
                    target.rotation = value;
                },
            endQuaternion, duration);

            tween.target = target;
            return tween;
        }

        public static TweenerCore<Quaternion, Quaternion> DoRotation(this RectTransform target, Quaternion endValue, float duration)
        {
            TweenerCore<Quaternion, Quaternion> tween = DOTween.To(
                () => target.rotation,
                (value) =>
                {
                    target.rotation = value;
                },
            endValue, duration);

            tween.target = target;
            return tween;
        }
    }
}