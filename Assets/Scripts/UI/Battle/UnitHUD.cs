namespace OMG.Unit.HUD
{
    using MaloProduction.CustomAttributes;

    using OMG.Unit.Status;

    using System.Collections.Generic;
    using System.Linq;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UnitHUD : MonoBehaviour
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
            [SerializeField] private Slider hit;

            [SerializeField] private RectTransform armorImage;
            [SerializeField] private TextMeshProUGUI armorText;
            [SerializeField] private TextMeshProUGUI lifeText;

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
                    lifeSlider.value = hp;
                    lifeText.text = $"{hp} / {maxHp}";

                    //TODO Make Tween with the hit slider
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
        #endregion

        [Title("Unit HUD")]
        [SerializeField] private LifeSlider lifeSlider;
        [SerializeField] private Status status;

        public void Initialize(Unit unit)
        {
            lifeSlider.Initialize(unit.Data);
            status.UpdateStatus(unit.Data.status);
            unit.OnUnitDataModified += UpdateHUD;
        }

        private void UpdateHUD(UnitData unitData)
        {
            lifeSlider.UpdateSlider(unitData);
            status.UpdateStatus(unitData.status);
        }
    }
}