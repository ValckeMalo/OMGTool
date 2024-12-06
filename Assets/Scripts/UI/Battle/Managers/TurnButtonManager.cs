namespace OMG.Battle.UI.Manager
{
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    [System.Serializable]
    public class TurnButtonManager
    {
        private enum TurnText
        {
            Oropo,
            Monsters,
        }

        private static string[] Converter = new string[2] { "END \nTURN", "MONSTERS \nTURN" };

        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI stateTurnText;
        [SerializeField] private TextMeshProUGUI turnIndexText;

        public void AddCallback(Action clickFct)
        {
            button.onClick.AddListener(() => clickFct());
        }

        public void OropoTurn()
        {
            Update(true, TurnText.Oropo);
        }
        public void MonstersTurn()
        {
            Update(false, TurnText.Monsters);
        }

        public void ToggleTurnButton(bool toggle)
        {
            button.interactable = toggle;
        }

        private void Update(bool isInteractible, TurnText turnText)
        {
            stateTurnText.text = Converter[(int)turnText];
            button.interactable = isInteractible;
            turnIndexText.text = "TURN " + BattleSystem.Instance.TurnIndex.ToString(); ;
        }
    }
}