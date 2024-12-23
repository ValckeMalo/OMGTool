namespace OMG.Battle.UI
{
    using MaloProduction.Tween;
    using MaloProduction.Tween.Core;
    using MaloProduction.Tween.DoTween.Module;

    using OMG.Battle.UI.Manager;
    using OMG.Unit;

    using System;
    using TMPro;
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

            public void UpdatePreviewBar(int previewWakfu)
            {
                if (tweenSlider != null)
                {
                    TweenManager.Despawn(tweenSlider);
                }

                tweenSlider = previewWakfuSlider.DoValue(previewWakfu, 0.2f);
            }
            public void ResetPreviewBar()
            {
                if (tweenSlider != null)
                {
                    TweenManager.Despawn(tweenSlider);
                }

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
        [SerializeField] private RectTransform turnName;
        [SerializeField] private TextMeshProUGUI turnNameText;

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

        public void TweenTest(bool isMonster)
        {
            const float stayTime = 1.5f;
            const float moveTime = 0.25f;
            const float fadeTime = 0.25f;

            turnNameText.text = isMonster ? "Monsters Turn" : "Player Turn";

            turnName.GetComponent<Image>().DoFade(0.7f, fadeTime);
            turnName.DoAnchorMove(new Vector2(0f, turnName.anchoredPosition.y), moveTime)
                .OnComplete(
                    () =>
                    {
                        turnName.DoAnchorMove(new Vector2(isMonster ? -500f : 500f, turnName.anchoredPosition.y), moveTime).AddDelay(stayTime);
                        turnName.GetComponent<Image>().DoFade(0f, fadeTime).AddDelay(stayTime);
                    });
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
        public void SpawnUnitHUD(Vector3 position, Unit unit, bool isMonster)
        {
            unitsHUDManager.SpawnUnitHUD(position, unit, isMonster);
        }
        #endregion
    }
}