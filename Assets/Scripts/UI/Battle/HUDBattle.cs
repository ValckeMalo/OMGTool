namespace OMG.Battle.UI
{
    using MaloProduction.Tween.Core;
    using MaloProduction.Tween.DoTween.Module;
    using OMG.Battle.UI.Manager;
    using OMG.Unit;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class HUDBattle : MonoBehaviour
    {
        #region WakfuGauge
        [System.Serializable]
        public class WakfuGauge
        {
            [SerializeField] private Slider wakfuSlider;
            [SerializeField] private Slider previewWakfuSlider;
            private TweenerCore<float, float> tweenSlider;

            [SerializeField] private Sprite[] padLockSprite = new Sprite[2];
            [SerializeField] private RectTransform padLocksContainer;

            private const int maxPadLocks = 3;
            private int nbActivePadLocks = maxPadLocks;

            #region Slider
            public void UpdateWakfuBar(int wakfu)
            {
                wakfuSlider.DoValue(wakfu, 0.2f);
            }

            public void UpdatePreviewBar(int previewWakfu, int wakfu)
            {
                if (tweenSlider != null)
                {
                    TweenManager.Despawn(tweenSlider);
                }

                tweenSlider = previewWakfuSlider.DoValue(previewWakfu, 0.2f);
                wakfuSlider.value = wakfu;
            }
            public void ResetPreviewBar(int wakfu)
            {
                if (tweenSlider != null)
                {
                    TweenManager.Despawn(tweenSlider);
                }

                previewWakfuSlider.value = 0f;

                wakfuSlider.value = wakfu;
            }
            public void ResetGauges()
            {
                ResetPreviewBar(0);
                wakfuSlider.value = 0f;
            }
            #endregion

            #region PadLock
            public void BreakPadLock()
            {
                padLocksContainer.GetChild(nbActivePadLocks - 1).GetComponentInChildren<Image>().sprite = padLockSprite[1];
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
        public static HUDBattle Instance => instance;
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
        [SerializeField] private TurnButtonManager turnButtonManager;
        [SerializeField] private WakfuGauge wakfuGauge;
        [SerializeField] private UnitsHUDManager unitsHUDManager; //NOT SURE ABOUT THIS CLASS IT'S SO EMPTY
        [SerializeField] private Button cancelSecondCard;

        public void Start()
        {
            cancelSecondCard.onClick.AddListener(() =>
            {
                ToggleSelectSecondCard(false);
                BattleSystem.Instance.GameBoard.CancelSecondCard();
            });
        }

        public static WakfuGauge OropoWakfuGauge { get => GetInstance().wakfuGauge; }

        public enum BattleHUDState
        {
            Oropo,
            Monsters,
        }

        public void SwitchState(BattleHUDState state)
        {
            switch (state)
            {
                case BattleHUDState.Oropo:
                    OropoMode();
                    break;

                case BattleHUDState.Monsters:
                    MonstersMode();
                    break;

                default:
                    Debug.Log("PB");
                    break;
            }
        }

        #region Mode
        private void OropoMode()
        {
            turnButtonManager.OropoTurn();

            ToggleSelectSecondCard(false);
            ToggleFinishersMode(false);
        }
        private void MonstersMode()
        {
            turnButtonManager.MonstersTurn();

            ToggleSelectSecondCard(false);
            ToggleFinishersMode(false);
        }
        #endregion

        #region Toggle UI
        public void ToggleSelectSecondCard(bool toggle)
        {
            cancelSecondCard.gameObject.SetActive(toggle);
        }
        public void ToggleFinishersMode(bool toggle)
        {
            turnButtonManager.ToggleTurnButton(!toggle);
        }
        #endregion

        #region UsefullFunction
        public void TurnButtonAddCallback(Action clickFct)
        {
            turnButtonManager.AddCallback(clickFct);
        }
        public void SpawnUnitHUD(Vector3 position, Unit unit)
        {
            unitsHUDManager.SpawnUnitHUD(position, unit);
        }
        #endregion
    }
}