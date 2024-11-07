using OMG.Card.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OMG.Battle
{
    public class HUDCombat : MonoBehaviour
    {
        public delegate void EventEndTurn();
        public static EventEndTurn OnEndTurn;

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

            private static string[] Converter = new string[2] { "FIN DU \nTOUR", "TOUR \nADVERSE" };

            public enum TurnText
            {
                Player,
                Enemy,
            }

            public override void Init()
            {
                button.onClick.AddListener(EndTurnButtonClicked);
                UpdateTurnButton(false, BattleSystem.TurnIndex, TurnText.Enemy);
            }

            private void OnPlayerTurn()
            {
                UpdateTurnButton(true, BattleSystem.TurnIndex, TurnText.Player);
            }

            private void EndTurnButtonClicked()
            {
                UpdateTurnButton(false, BattleSystem.TurnIndex, TurnText.Enemy);
                OnEndTurn?.Invoke();
            }

            private void UpdateTurnButton(bool isInteractible, int indexTurn, TurnText turnText)
            {
                stateTurnText.text = Converter[(int)turnText];
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
        #region Preview Wakfu Bar
        [System.Serializable]
        private class PreviewWakfuBar : UIClass
        {
            [SerializeField] private Slider slider;
            private const int maxWakfuBar = 6;

            public override void Init()
            {
                slider.value = 0;
                slider.minValue = 0;
                slider.maxValue = maxWakfuBar;
                slider.wholeNumbers = true;
            }

            public void UpdatePreviewWakfuBar(PlayableCard card, int wakfuUsed, MouseState state)
            {
                if (state == MouseState.BeginOver)
                    slider.value = Mathf.Min(card.Wakfu + wakfuUsed, 6);
                else if (state == MouseState.ExitOver)
                    slider.value = 0;
            }
        }
        #endregion
        #endregion

        [Header("UI Class")]
        [SerializeField] private TurnButton turnButton;
        [Space(2f)]
        [SerializeField] private WakfuBar wakfuBar;
        [SerializeField] private PreviewWakfuBar previewWakfuBar;

        public void Awake()
        {
            turnButton.Init();
            wakfuBar.Init();
            previewWakfuBar.Init();
        }
    }
}