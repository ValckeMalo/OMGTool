namespace OMG.Unit.Monster
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "Arakne", menuName = "Unit/Monster/Arakne", order = 0)]
    public class Arakne : Monster
    {
        public override void Action()
        {
        }

        public override void SearchNextAction()
        {
        }

        public override string GetName()
        {
            return "Arakne";
        }
    }
}