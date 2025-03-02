namespace OMG.Game.Fight.Entities
{
    using MVProduction.CustomAttributes;
    using MVProduction.Tween.Core;
    using MVProduction.Tween.DoTween.Module;

    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    using OMG.Game.Tooltip;
    using OMG.Data.Mobs.Actions;
    
    public class FightMobEntityUI : FightEntityUI
    {
        [Title("Preview Attack")]
        [SerializeField] private CanvasGroup previewAttackGroup;
        [SerializeField] private TextMeshProUGUI previewAttackText;
        [SerializeField] private Image previewAttackIcon;
        private MobActionUI previewAttackUI;

        //tween preview attack
        private TweenerCore<float, float> previewAttackTween = null;

        #region Tooltip
        protected override List<TooltipData> GetAllTooltips()
        {
            List<TooltipData> allTooltips = new List<TooltipData>();
            allTooltips.Add(GetAttackTooltip());
            allTooltips.AddRange(GetStatusTooltip());
            return allTooltips;
        }
        private TooltipData GetAttackTooltip()
        {
            return new TooltipData(TooltipType.ACTION, previewAttackUI.Title, previewAttackUI.Description, previewAttackUI.Icon);
        }
        #endregion

        #region Preview Attack
        public void NewAttack(int attackValue, MobActionUI data)
        {
            previewAttackUI = data;
            previewAttackText.text = attackValue.ToString();
            previewAttackIcon.sprite = previewAttackUI.Icon;
            FadePreviewAttack(0f, 0.1f);
        }
        public void UpdateAttackUI(int newAttackValue)
        {
            previewAttackText.text = newAttackValue.ToString();
        }
        public void AttackUsed()
        {
            FadePreviewAttack(0f, 0.1f);
        }
        private void FadePreviewAttack(float endValue, float duration)
        {
            if (previewAttackTween != null)
                TweenManager.Despawn(previewAttackTween);

            previewAttackTween = previewAttackGroup.DoFade(endValue, duration);
        }
        #endregion
    }
}