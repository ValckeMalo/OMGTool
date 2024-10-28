using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OMG.Battle
{
    public class HUDCombat : MonoBehaviour
    {
        [System.Serializable]
        private class TurnButton
        {
            public Button button;
            public TextMeshProUGUI stateTurnText;
            public TextMeshProUGUI turnIndexText;

            public static string[] Converter = new string[2] { "FIN DU \nTOUR", "TOUR \nADVERSE" };
        }

        [SerializeField] private TurnButton turnButton;

        public void Awake()
        {
            turnButton.button.onClick.AddListener(EndTurnButtonClicked);
            UpdateTurnButton(false, BattleSystem.TurnIndex, 0);

            BattleSystem.OnPlayerTurn += OnPlayerTurn;
        }

        private void OnPlayerTurn(PlayerBattleState state)
        {
            UpdateTurnButton(true, BattleSystem.TurnIndex, 0);
        }

        private void EndTurnButtonClicked()
        {
            UpdateTurnButton(false, BattleSystem.TurnIndex, 1);
            BattleSystem.OnNextTurn?.Invoke(StateTurn.Ennemi);
        }

        private void UpdateTurnButton(bool isInteractible, int indexTurn, int indexString)
        {
            turnButton.stateTurnText.text = TurnButton.Converter[indexString];
            turnButton.button.interactable = isInteractible;
            turnButton.turnIndexText.text = "TOUR " + indexTurn.ToString(); ;
        }
    }
}