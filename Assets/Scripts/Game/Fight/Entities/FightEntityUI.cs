namespace OMG.Game.Fight.Entities
{
    using System.Collections;
    using System.Collections.Generic;

    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    using OMG.Game.Tooltip;

    using MVProduction.Tween;
    using MVProduction.Tween.Core;
    using MVProduction.CustomAttributes;
    using MVProduction.Tween.DoTween.Module;

    public class FightEntityUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Title("Health")]
        [Header("Health Slider Color")]
        [SerializeField] private Color healthColor;
        [SerializeField] private Color healthBackgroundColor;
        [SerializeField] private Color armorColor;
        [SerializeField] private Color armorBackgroundColor;

        [Header("Health Slider")]
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Slider hitSlider;
        [SerializeField] private Image healthFill;
        [SerializeField] private Image healthBackground;
        [SerializeField] private TextMeshProUGUI healthText;

        [Header("Armor")]
        [SerializeField] private CanvasGroup armorGroup;
        [SerializeField] private TextMeshProUGUI armorText;

        //tween slider
        private TweenerCore<float, float> hitSliderTween = null;
        private TweenerCore<float, float> healthSliderTween = null;

        [Title("Status")]
        [SerializeField] private CanvasGroup statusGroup;
        [SerializeField] private Transform containerStatusUI;
        [SerializeField] private GameObject StatusUIPrefab;
        private Dictionary<StatusType, FightEntityStatusUI> dicStatusUI = new Dictionary<StatusType, FightEntityStatusUI>();

        [Title("Hover")]
        [SerializeField] private CanvasGroup hoverGroup;
        [SerializeField] private TextMeshProUGUI entityName;

        //hover Tween
        private TweenerCore<float, float> hoverTween = null;

        [Title("Tooltip")]
        [SerializeField] private CanvasGroup tooltipGroup;
        [SerializeField] private Transform containerTooltip;
        private const float tooltipActivationDelay = 1f;
        private List<GameObject> allTooltipsObject = null;

        //tooltip tween
        private TweenerCore<float, float> tooltipTween = null;

        #region Animation
        public IEnumerator AnimationDeath()
        {
            //TODO add the fx et death mob etc

            yield return new WaitForSeconds(3f);//time anim/fx death
        }
        #endregion

        #region Health Slider
        public void InitHealthUI(int currentHealth, int maxHealth, int currentArmor)
        {
            healthSlider.maxValue = maxHealth;
            hitSlider.maxValue = maxHealth;

            UpdateHealthUI(currentHealth, currentArmor);
        }
        public void UpdateHealthUI(int currentHealth, int currentArmor)
        {
            if (currentArmor > 0)
            {
                UpdateColorsHealth(true);
                armorGroup.alpha = 1f;
                armorText.text = currentArmor.ToString();
            }
            else
            {
                UpdateColorsHealth(false);
                armorGroup.alpha = 0f;
            }

            healthText.text = currentHealth.ToString();
            UpdateSliderTween(currentHealth);
        }

        private void UpdateSliderTween(int targetValue)
        {
            //If the previous tween was not complete despawn it to recreate them
            if (healthSliderTween != null) TweenManager.Despawn(healthSliderTween);
            if (hitSliderTween != null) TweenManager.Despawn(hitSliderTween);

            //animate the slider
            healthSliderTween = healthSlider.DoValue(targetValue, 0.1f);
            hitSliderTween = hitSlider.DoValue(targetValue, 0.5f).AddDelay(0.35f).SetEase(MVProduction.Tween.Ease.Easing.OutCubic);
        }
        private void UpdateColorsHealth(bool hasArmor)
        {
            if (hasArmor)
            {
                healthFill.color = armorColor;
                healthBackground.color = armorBackgroundColor;
            }
            else
            {
                healthFill.color = healthColor;
                healthBackground.color = healthBackgroundColor;
            }
        }
        #endregion

        #region Status
        public void UpdateAllStatusUI(List<FightEntityStatus> entityStatusCollections)
        {
            foreach (FightEntityStatus entityStatus in entityStatusCollections)
            {
                UpdateStatusUI(dicStatusUI[entityStatus.Status], entityStatus.Turn);
            }
        }
        private void UpdateStatusUI(FightEntityStatusUI statusUI, int turnValue)
        {
            statusUI.UpdateTextTurn(turnValue);
        }
        public void AddStatusUI(FightEntityStatus newStatus)
        {
            if (dicStatusUI.ContainsKey(newStatus.Status))
            {
                UpdateStatusUI(dicStatusUI[newStatus.Status], newStatus.Turn);
                return;
            }

            //create a new status ui and add it in the dic
            GameObject statusUIObject = Instantiate(StatusUIPrefab, containerStatusUI);
            FightEntityStatusUI statusUI = statusUIObject.GetComponent<FightEntityStatusUI>();
            statusUI.Initialize(newStatus.Status, newStatus.Turn);
            dicStatusUI.Add(newStatus.Status, statusUI);
        }
        public void RemoveStatusUI(StatusType removeStatusType)
        {
            if (!dicStatusUI.ContainsKey(removeStatusType))
            {
                return;
            }

            Destroy(dicStatusUI[removeStatusType].gameObject);
            dicStatusUI.Remove(removeStatusType);
        }
        #endregion

        #region IPointer
        public void OnPointerEnter(PointerEventData eventData)
        {
            FadeHover(0.9f, 0.1f);
            ShowTooltip();
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            FadeHover(0f, 0.05f);
            HideTooltip();
        }

        private void FadeHover(float endValue, float duration)
        {
            if (hoverTween != null)
                TweenManager.Despawn(hoverTween);

            hoverTween = hoverGroup.DoFade(endValue, duration);
        }
        #endregion

        #region Tooltip
        private void ShowTooltip()
        {
            if (tooltipTween != null)
                TweenManager.Despawn(tooltipTween);

            SetTooltip(GetAllTooltips());
            tooltipTween = tooltipGroup.DoFade(1f, 0.075f).AddDelay(tooltipActivationDelay);
        }
        private void HideTooltip()
        {
            if (tooltipTween != null)
                TweenManager.Despawn(tooltipTween);

            ReleaseTooltip();
            tooltipTween = tooltipGroup.DoFade(0f, 0.05f);
        }
        protected virtual List<TooltipData> GetAllTooltips()
        {
            return GetStatusTooltip();
        }
        protected List<TooltipData> GetStatusTooltip()
        {
            List<TooltipData> statusTooltip = new List<TooltipData>();

            if (dicStatusUI.Count <= 0)
                return statusTooltip;

            foreach (FightEntityStatusUI statusUI in dicStatusUI.Values)
            {
                statusTooltip.Add(new TooltipData(TooltipType.STATE, statusUI.Title, statusUI.Description, statusUI.Icon));
            }

            return statusTooltip;
        }
        private void SetTooltip(List<TooltipData> allTooltipData)
        {
            List<GameObject> tooltipsObject = TooltipManager.OnSpawnTooltip?.Invoke(allTooltipData);
            if (tooltipsObject != null && tooltipsObject.Count > 0)
            {
                allTooltipsObject = tooltipsObject;
                foreach (GameObject tooltipObject in tooltipsObject)
                {
                    if (tooltipObject == null)
                    {
                        Debug.LogError("One of the tooltip of the entity is null");
                        continue;
                    }

                    tooltipObject.transform.SetParent(containerTooltip, false);
                }
            }
        }
        private void ReleaseTooltip()
        {
            if (allTooltipsObject != null)
            {
                TooltipManager.OnReleaseTooltipObject?.Invoke(allTooltipsObject);
                allTooltipsObject.Clear();
                allTooltipsObject = null;
            }
        }
        #endregion
    }
}