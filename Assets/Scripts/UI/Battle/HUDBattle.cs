namespace OMG.Battle.UI
{
    using MaloProduction.CustomAttributes;
    using MaloProduction.Tween.DoTween.Module;

    using OMG.Unit;
    using OMG.Unit.HUD;

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

            public void AddCallback(System.Action action)
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
            public void DisableButton()
            {
                button.interactable = false;
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
        [System.Serializable]
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
                wakfuSlider.DoValue(wakfu, 0.2f);
            }

            public void UpdatePreviewBar(int previewWakfu)
            {
                previewWakfuSlider.DoValue(previewWakfu, 0.2f);
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

        #region Unit HUD
        [System.Serializable]
        public class UnitsHUD
        {
            [Title("Unit HUD")]
            [SerializeField] private RectTransform parent;
            [SerializeField] private RectTransform canvasTransform;
            [SerializeField] private GameObject prefabUnitHUD;

            public void SpawnUnitHUD(Vector3 unitPosition, Unit unit)
            {
                Vector3 viewportPosition = Camera.main.WorldToViewportPoint(unitPosition);
                Vector2 worldObjectScreenPosition = new Vector2(
                                        ((viewportPosition.x * canvasTransform.sizeDelta.x) - (canvasTransform.sizeDelta.x * 0.5f)),
                                        ((viewportPosition.y * canvasTransform.sizeDelta.y) - (canvasTransform.sizeDelta.y * 0.5f)));

                GameObject unitHUD = Instantiate(prefabUnitHUD, parent);
                unitHUD.GetComponent<RectTransform>().anchoredPosition = worldObjectScreenPosition;
                unitHUD.GetComponent<UnitHUD>().Initialize(unit);
            }
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
        [SerializeField] private UnitsHUD unitsHUD;

        public static TurnButton EndTurnButton { get => GetInstance().turnButton; }
        public static WakfuGauge OropoWakfuGauge { get => GetInstance().wakfuGauge; }
        public static UnitsHUD UnitHUD { get => GetInstance().unitsHUD; }
    }
}