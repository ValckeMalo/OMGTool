namespace OMG.Game.Fight.Entities
{
    using UnityEngine;

    public class FightEntityUI : MonoBehaviour
    {
        public delegate void EventUpdateUI();
        protected EventUpdateUI OnUpdateUI;

        public void RegisterOnUpdateUI(EventUpdateUI action)
        {
            OnUpdateUI += action;
        }
        public void UnRegisterOnUpdateUI(EventUpdateUI action)
        {
            OnUpdateUI -= action;
        }
    }
}