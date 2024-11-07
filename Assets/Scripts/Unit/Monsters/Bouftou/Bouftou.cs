using UnityEngine;

namespace OMG.Unit.Monster
{
    [CreateAssetMenu(fileName = "Bouftou", menuName = "Unit/Monster/Bouftou", order = 1)]
    public class Bouftou : Monster
    {
        public override void Action()
        {
        }

        public override void SearchNextAction()
        {
        }

        public override string GetName()
        {
            return "Bouftou";
        }
    }
}