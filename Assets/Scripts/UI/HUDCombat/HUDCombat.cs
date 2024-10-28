using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OMG.Battle
{
    public class HUDCombat : MonoBehaviour
    {
        #region Class
        #region UI Class
        private abstract class UIClass
        {
            public abstract void Init();
        }
        #endregion
        #region Turn Button
        [System.Serializable]
        private class TurnButton : UIClass
        {
            [SerializeField] private Button button;
            [SerializeField] private TextMeshProUGUI stateTurnText;
            [SerializeField] private TextMeshProUGUI turnIndexText;

            [SerializeField] private static string[] Converter = new string[2] { "FIN DU \nTOUR", "TOUR \nADVERSE" };

            public enum TurnText
            {
                Player,
                Enemy,
            }

            public override void Init()
            {
                button.onClick.AddListener(EndTurnButtonClicked);
                UpdateTurnButton(false, BattleSystem.TurnIndex, TurnButton.TurnText.Enemy);

                BattleSystem.OnPlayerTurn += OnPlayerTurn;
            }

            private void OnPlayerTurn(PlayerBattleState state)
            {
                UpdateTurnButton(true, BattleSystem.TurnIndex, TurnButton.TurnText.Player);
            }

            private void EndTurnButtonClicked()
            {
                UpdateTurnButton(false, BattleSystem.TurnIndex, TurnButton.TurnText.Enemy);
                BattleSystem.OnNextTurn?.Invoke(StateTurn.Ennemi);
            }

            private void UpdateTurnButton(bool isInteractible, int indexTurn, TurnButton.TurnText turnText)
            {
                stateTurnText.text = TurnButton.Converter[(int)turnText];
                button.interactable = isInteractible;
                turnIndexText.text = "TOUR " + indexTurn.ToString(); ;
            }
        }
        #endregion
        #region Wakfu Bar
        [System.Serializable]
        private class WakfuBar : UIClass
        {
            [SerializeField] private Slider slider;
            [SerializeField] private RectTransform padLocks;
            private int maxWakfu = 3;
            private const int nbPadlocks = 3;
            private const int maxWakfuBar = 6;

            public override void Init()
            {
                slider.value = 0;
                slider.minValue = 0;
                slider.maxValue = maxWakfuBar;
                slider.wholeNumbers = true;

                PlayerBattleSystem.OnWakfuUse += UpdateWakfuBar;
            }

            public void UpdateWakfuBar(int totalAmount, int maxWakfu)
            {
                slider.value = totalAmount;

                if (this.maxWakfu < maxWakfu)
                {
                    this.maxWakfu = maxWakfu;
                    int nbToDisable = this.maxWakfu - nbPadlocks;

                    for (int i = 0; i < nbToDisable; i++)
                    {
                        padLocks.GetChild(i).gameObject.SetActive(false);
                    }
                }
                else if (this.maxWakfu > maxWakfu)
                {
                    this.maxWakfu = maxWakfu;
                    int nbToEnable = maxWakfuBar - this.maxWakfu;

                    for (int i = 0; i < nbToEnable; i++)
                    {
                        padLocks.GetChild(i).gameObject.SetActive(true);
                    }
                }
            }
        }
        #endregion
        #endregion

        [Header("UI Class")]
        [SerializeField] private TurnButton turnButton;
        [Space(2f)]
        [SerializeField] private WakfuBar wakfuBar;

        public void Awake()
        {
            turnButton.Init();
            wakfuBar.Init();
        }
    }
}