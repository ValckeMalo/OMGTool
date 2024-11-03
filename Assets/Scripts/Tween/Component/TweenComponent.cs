namespace MaloProduction.Tween.Component
{
    using MaloProduction.Tween.Core;
    using UnityEngine;

    public class TweenComponent : MonoBehaviour
    {
        void Update()
        {
            TweenManager.Update(Time.deltaTime);
        }
    }
}