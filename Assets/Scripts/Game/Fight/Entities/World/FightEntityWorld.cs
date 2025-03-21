namespace OMG.Game.Fight.Entities
{
    using MVProduction.CustomAttributes;
    using MVProduction.Tween;
    using MVProduction.Tween.DoTween.Module;
    using MVProduction.Tween.Ease;
    using UnityEngine;

    public class FightEntityWorld : MonoBehaviour
    {
        [Title("World")]
        [SerializeField] private Transform worldTransform;
        [SerializeField] private Animator entityAnimator;

        public Transform WorldTransform => worldTransform;

        public void MoveTo(Vector3 newWorldPos,float duration)
        {
            worldTransform.DoMove(newWorldPos, duration).SetEase(Easing.InOutQuint);
        }

        public float AttackAnim()
        {
            entityAnimator.SetTrigger("Attack");
            return 1.75f;
        }
    }
}