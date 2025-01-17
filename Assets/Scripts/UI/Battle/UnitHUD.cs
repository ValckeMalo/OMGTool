namespace OMG.Unit.HUD
{
    using MaloProduction.CustomAttributes;
    using MaloProduction.Tween;
    using MaloProduction.Tween.Core;
    using MaloProduction.Tween.DoTween.Module;

    using OMG.Battle.UI.Tooltip;
    using OMG.Unit.Status;

    using System.Collections;
    using System.Collections.Generic;
    using TMPro;

    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class UnitHUD : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region Class
        [System.Serializable]
        private class LifeSlider
        {
            private enum SliderState
            {
                Life,
                Armor,
            }

            [Title("Life Slider")]
            [SerializeField] private Color lifeColor;
            [SerializeField] private Color lifeBackgroundColor;

            [SerializeField] private Color armorColor;
            [SerializeField] private Color armorBackgroundColor;

            [SerializeField] private Slider lifeSlider;
            [SerializeField] private Image lifeBackground;
            [SerializeField] private Image lifeFill;
            [SerializeField] private Slider hitSlider;

            [SerializeField] private RectTransform armorImage;
            [SerializeField] private TextMeshProUGUI armorText;
            [SerializeField] private TextMeshProUGUI lifeText;

            private TweenerCore<float, float> hitSliderTween = null;
            private TweenerCore<float, float> lifeSliderTween = null;

            private SliderState cState = SliderState.Life;

            public void Initialize(UnitData data)
            {
                lifeSlider.maxValue = data.maxHp;

                SetStateLife();
                UpdateSlider(data);
            }

            public void UpdateSlider(UnitData data)
            {
                if (data.armor > 0)
                {
                    if (cState != SliderState.Armor)
                    {
                        SetStateArmor();
                    }

                    UpdateArmorText(data.armor);
                }
                else if (cState != SliderState.Life)
                {
                    SetStateLife();
                }

                UpdateLifeSlider(data.hp, data.maxHp);
            }

            private void SetStateArmor()
            {
                lifeFill.color = armorColor;
                lifeBackground.color = armorBackgroundColor;

                armorImage.gameObject.SetActive(true);

                cState = SliderState.Armor;
            }

            private void SetStateLife()
            {
                lifeBackground.color = lifeBackgroundColor;
                lifeFill.color = lifeColor;

                armorImage.gameObject.SetActive(false);

                cState = SliderState.Life;
            }

            private void UpdateArmorText(int armor)
            {
                armorText.text = armor.ToString();
            }

            private void UpdateLifeSlider(int hp, int maxHp)
            {
                if (hp != (int)lifeSlider.value)
                {
                    hp = Mathf.Max(0, hp);// clamp the value to not go under 0
                    lifeText.text = $"{hp} / {maxHp}";// update text

                    //If the previous tween was not complete despawn it to recreate them
                    if (lifeSliderTween != null) TweenManager.Despawn(lifeSliderTween);
                    if (hitSliderTween != null) TweenManager.Despawn(hitSliderTween);

                    //tween/animate the value
                    lifeSliderTween = lifeSlider.DoValue(hp, 0.1f);
                    hitSliderTween = hitSlider.DoValue(hp, 0.5f).AddDelay(0.35f).SetEase(MaloProduction.Tween.Ease.Easing.OutCubic);
                }
            }
        }
        [System.Serializable]
        private class Status
        {
            [Title("Status")]
            [SerializeField] private RectTransform parent; // Parent UI container for the status elements
            [SerializeField] private GameObject statusPrefab; // Prefab for creating new status UI elements
            private List<StatusUI> allStatus = new List<StatusUI>(); // List holding all active StatusUI instances

            /// <summary>
            /// Updates the UI to reflect the provided list of UnitStatus.
            /// Adds new statuses, updates existing ones, and removes outdated ones.
            /// </summary>
            /// <param name="unitStatuses">List of current UnitStatus data</param>
            public void UpdateStatus(List<UnitStatus> unitStatuses)
            {
                //Create a dictionary for fast lookup of existing StatusUI by their status identifier
                Dictionary<string, StatusUI> statusUIDict = new Dictionary<string, StatusUI>();
                foreach (StatusUI statusUI in allStatus)
                {
                    statusUIDict[statusUI.Status.ToString()] = statusUI;
                }

                //Track which StatusUI elements are still in use
                HashSet<StatusUI> usedUI = new HashSet<StatusUI>();

                foreach (UnitStatus currentUnitStatus in unitStatuses)
                {
                    // Check if the status already exists in the UI
                    if (statusUIDict.TryGetValue(currentUnitStatus.status.ToString(), out StatusUI statusUI))
                    {
                        // Update the existing UI element
                        UpdateStatusUI(statusUI, currentUnitStatus);
                        usedUI.Add(statusUI);
                    }
                    else
                    {
                        // Add a new UI element if it doesn't exist
                        StatusUI newStatusUI = AddStatusUI(currentUnitStatus);
                        usedUI.Add(newStatusUI);
                    }
                }

                //Remove any UI elements that are no longer needed
                RemoveUnusedStatusUI(usedUI);
            }

            /// <summary>
            /// Creates a new StatusUI element and initializes it with the given UnitStatus data.
            /// </summary>
            /// <param name="unitStatus">The UnitStatus data to initialize the UI element with</param>
            /// <returns>The newly created StatusUI</returns>
            private StatusUI AddStatusUI(UnitStatus unitStatus)
            {
                // Instantiate a new StatusUI element from the prefab
                StatusUI statusUI = Instantiate(statusPrefab, parent).GetComponent<StatusUI>();
                statusUI.Initialize(unitStatus.turn, unitStatus.status);
                allStatus.Add(statusUI); // Add it to the active UI list
                return statusUI;
            }

            /// <summary>
            /// Removes any StatusUI elements that are no longer used.
            /// </summary>
            /// <param name="usedUI">HashSet of StatusUI elements still in use</param>
            private void RemoveUnusedStatusUI(HashSet<StatusUI> usedUI)
            {
                // Create a temporary list to track items to remove (to avoid modifying the list while iterating)
                List<StatusUI> toRemove = new List<StatusUI>();

                foreach (StatusUI statusUI in allStatus)
                {
                    if (!usedUI.Contains(statusUI))
                    {
                        toRemove.Add(statusUI);
                    }
                }

                // Remove and destroy unused StatusUI elements
                foreach (StatusUI statusUI in toRemove)
                {
                    allStatus.Remove(statusUI);
                    statusUI.Destroy();
                }
            }

            /// <summary>
            /// Updates an existing StatusUI element with the latest data from UnitStatus.
            /// </summary>
            /// <param name="statusUI">The UI element to update</param>
            /// <param name="currentUnitStatus">The updated data</param>
            private void UpdateStatusUI(StatusUI statusUI, UnitStatus currentUnitStatus)
            {
                statusUI.UpdateTextTurn(currentUnitStatus.turn); // Update the UI element with the new turn count
            }
        }
        [System.Serializable]
        private class PreviewAttack
        {
            [Title("Preview Attack")]
            [SerializeField] public GameObject previewAttack;
            [SerializeField] private TextMeshProUGUI textMesh;
            private bool visibility = false;
            public bool Visibility => visibility;

            public void UpdatePreview(int value) => textMesh.text = value.ToString();
            public void ToogleVisibility(bool toggle)
            {
                visibility = toggle;
                previewAttack.SetActive(toggle);
            }
        }
        #endregion

        [Title("HUD Settings")]
        [SerializeField] private Image[] cornerHoverImage;
        [SerializeField] private RectTransform startTooltipPos;
        private float timeShowTooltip = 1f;

        //previewAttack
        private string attackHeaderDescription = string.Empty;
        private string attackDescription = string.Empty;

        //Tween
        TweenerCore<float, float>[] cornerFadeTween = null;
        TweenerCore<float, float> nameFadeTween = null;

        [Header("Top")]
        [SerializeField] private PreviewAttack previewAttack;
        [SerializeField] private TextMeshProUGUI nameUnit;

        [Header("Body")]
        [SerializeField] private LayoutElement body;

        [Header("Bottom")]
        [SerializeField] private LifeSlider lifeSlider;
        [SerializeField] private Status status;

        public void Initialize(Unit unit, float sizeUnit, bool isMonster)
        {
            lifeSlider.Initialize(unit.Data);
            status.UpdateStatus(unit.Data.status);
            unit.OnUnitDataModified += UpdateHUD;

            previewAttack.ToogleVisibility(isMonster);
            body.preferredHeight = sizeUnit;

            if (cornerHoverImage == null || cornerHoverImage.Length <= 0)
                Debug.LogError($"In UnitHUD the cornerHoverImage was empty or set to null.");

            cornerFadeTween = new TweenerCore<float, float>[cornerHoverImage.Length];
            FadeAllCorner(0f, 0.01f);
            FadeMoreInfoUnit(0f, 0.01f);

            nameUnit.text = unit.GetName();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void UpdatePreviewNextAttack(int value, string tooltipDesc, string attackHeaderDescription)
        {
            attackDescription = tooltipDesc;
            this.attackHeaderDescription = attackHeaderDescription;

            previewAttack.UpdatePreview(value);
        }

        private void UpdateHUD(UnitData unitData)
        {
            lifeSlider.UpdateSlider(unitData);
            status.UpdateStatus(unitData.status);
        }

        #region IPointer
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (previewAttack.Visibility)
                StartCoroutine(HoverInfo());

            FadeAllCorner(0.9f, 0.1f);
        }
        private IEnumerator HoverInfo()
        {
            yield return new WaitForSeconds(timeShowTooltip);

            FadeMoreInfoUnit(0.9f, 0.1f);
            TooltipManager.Instance.ShowTooltip(attackHeaderDescription, attackDescription, startTooltipPos.position);
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            StopAllCoroutines();

            FadeAllCorner(0f, 0.05f);
            FadeMoreInfoUnit(0f, 0.05f);

            TooltipManager.Instance.HideTooltipCard();
        }

        private void FadeAllCorner(float endValue, float duration)
        {
            for (int i = 0; i < cornerHoverImage.Length; i++)
            {
                if (cornerFadeTween[i] != null)
                    TweenManager.Despawn(cornerFadeTween[i]);

                cornerFadeTween[i] = cornerHoverImage[i].DoFade(endValue, duration);
            }
        }
        private void FadeMoreInfoUnit(float endValue, float duration)
        {
            if (nameFadeTween != null)
                TweenManager.Despawn(nameFadeTween);

            nameFadeTween = nameUnit.DoFade(endValue, duration);
        }
        #endregion
    }
}