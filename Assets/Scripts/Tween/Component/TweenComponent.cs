namespace MVProduction.Tween.Component
{
    using MVProduction.Tween.Core;
    using UnityEngine;

    public class TweenComponent : MonoBehaviour
    {
        void Update()
        {
            TweenManager.Update(Time.deltaTime);
        }
    }
}