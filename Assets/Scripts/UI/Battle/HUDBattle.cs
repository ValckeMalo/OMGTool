namespace OMG.Battle.UI
{
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class HUDBattle : MonoBehaviour
    {
        #region Turn Button
        [System.Serializable]
        public class TurnButton
        {
            private enum TurnText
            {
                Player,
                Monster,
            }
            private static string[] Converter = new string[2] { "FIN DU \nTOUR", "TOUR \nADVERSE" };

            [SerializeField] private Button button;
            [SerializeField] private TextMeshProUGUI stateTurnText;
            [SerializeField] private TextMeshProUGUI turnIndexText;

            public void AddCallback(Action action)
            {
                button.onClick.AddListener(() => action());
            }
            public void PlayerTurnButton()
            {
                Update(true, BattleSystem.TurnIndex, TurnText.Player);
            }
            public void MonstersTurnButton()
            {
                Update(true, BattleSystem.TurnIndex, TurnText.Monster);
            }

            private void Update(bool isInteractible, int turnIndex, TurnText turnText)
            {
                stateTurnText.text = Converter[(int)turnText];
                button.interactable = isInteractible;
                turnIndexText.text = "TOUR " + turnIndex.ToString(); ;
            }
        }
        #endregion

        #region WakfuGauge
        [Serializable]
        public class WakfuGauge
        {
            [SerializeField] private Slider wakfuSlider;
            [SerializeField] private Slider previewWakfuSlider;

            [SerializeField] private Sprite[] padLockSprite = new Sprite[2];
            [SerializeField] private RectTransform padLocksContainer;

            private const int maxPadLocks = 3;
            private int nbActivePadLocks = maxPadLocks;

            #region Slider
            public void UpdateWakfuBar(int wakfu)
            {
                wakfuSlider.value = wakfu;
                //TODO module de tween pour les sliders
            }

            public void UpdatePreviewBar(int previewWakfu)
            {
                previewWakfuSlider.value = previewWakfu;
                //TODO module de tween pour les sliders
            }
            public void ResetPreviewBar()
            {
                previewWakfuSlider.value = 0f;
            }
            public void ResetGauges()
            {
                ResetPreviewBar();
                wakfuSlider.value = 0f;
            }
            #endregion

            #region PadLock
            public void BreakPadLock()
            {
                padLocksContainer.GetChild(nbActivePadLocks - 1).GetComponent<Image>().sprite = padLockSprite[1];
            }
            public void ResetPadLock()
            {
                nbActivePadLocks = maxPadLocks;
                EnableAllPadLocks();
            }
            public void RemovePadLock()
            {
                DisablePadLock(GetPadLock());
            }
            private void EnableAllPadLocks()
            {
                for (int i = 0; i < maxPadLocks; i++)
                {
                    EnablePadLock(GetPadLock(i));
                }
            }
            private void EnablePadLock(GameObject padLock)
            {
                padLock.SetActive(true);
            }
            private void DisableAllPadLocks()
            {
                for (int i = 0; i < maxPadLocks; i++)
                {
                    DisablePadLock(GetPadLock(i));
                }
            }
            private void DisablePadLock(GameObject padLock)
            {
                padLock.SetActive(false);
                nbActivePadLocks--;
            }
            private GameObject GetPadLock()
            {
                return padLocksContainer.GetChild(nbActivePadLocks - 1).gameObject;
            }
            private GameObject GetPadLock(int index)
            {
                return padLocksContainer.GetChild(index).gameObject;
            }
            #endregion
        }
        #endregion

        #region Singleton
        private static HUDBattle instance = null;
        private static HUDBattle GetInstance()
        {
            if (instance == null)
            {
                Debug.LogError($"The instance of HUDCombat doesn't exist");
                return null;
            }

            return instance;
        }
        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
        }
        #endregion

        [Header("UI Class")]
        [SerializeField] private TurnButton turnButton;
        [SerializeField] private WakfuGauge wakfuGauge;

        public static TurnButton EndTurnButton { get => GetInstance().turnButton; }
        public static WakfuGauge PlayerWakfuGauge { get => GetInstance().wakfuGauge; }
    }
}