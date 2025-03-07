namespace OMG.Game.Fight.Entities
{
    using MVProduction.CustomAttributes;
    using MVProduction.Tween;
    using MVProduction.Tween.Core;
    using MVProduction.Tween.Ease;
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

        //tween move UI
        const float timeMove = 1f;

        //tween preview attack
        private Tween previewAttackTween = null;

        //tween update Pos
        private Tween movePosTween = null;

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
            FadePreviewAttack(1f, 0.1f);
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
                previewAttackTween = TweenManager.Despawn(previewAttackTween);

            previewAttackTween = previewAttackGroup.DoFade(endValue, duration);
        }
        #endregion

        public void UpdatePos(Vector2 canvasPos)
        {
            if (movePosTween != null)
                movePosTween = TweenManager.Despawn(movePosTween);

            movePosTween = (transform as RectTransform).DoAnchorMove(canvasPos, timeMove).SetEase(Easing.InOutQuint);
        }

        public override void DespawnUI()
        {
            if (previewAttackTween != null) 
                previewAttackTween = TweenManager.Despawn(previewAttackTween);

            if (movePosTween != null)
                movePosTween = TweenManager.Despawn(movePosTween);

            base.DespawnUI();
        }
    }
}