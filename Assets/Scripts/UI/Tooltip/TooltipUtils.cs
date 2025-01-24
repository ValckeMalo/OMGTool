namespace OMG.Battle.UI.Tooltip
{
    using static OMG.Battle.UI.Tooltip.TooltipManager;

    using static OMG.Unit.HUD.StatusUIData;
    using OMG.Unit.Status;
    using OMG.Unit.HUD;

    using static OMG.Card.UI.CardUIData;
    using OMG.Card.Data;
    using OMG.Card.UI;

    using System.Collections.Generic;
    using UnityEngine;

    public static class TooltipUtils
    {
        private static StatusUIData statusUIData = null;
        private static string statusUiDataPath = "Runtime/UI/StatusUIData";

        private static CardUIData cardUIData = null;
        private static string cardUIDataPath = "Runtime/UI/CardUIData";

        public static TooltipData[] UnitStateToTooltipData(List<UnitStatus> status)
        {
            if (statusUIData == null)
            {
                statusUIData = Resources.Load<StatusUIData>(statusUiDataPath);
                if (statusUIData == null)
                {
                    Debug.LogError($"The status ui data is untraceable is normally at this path : Assets/Ressources/{statusUiDataPath}");
                    return null;
                }
            }

            if (status == null || status.Count <= 0)
            {
                Debug.LogError($"The status of the unit send is null or empty : {status}");
                return null;
            }

            List<TooltipData> tooltipDatas = new List<TooltipData>();

            foreach (UnitStatus unitStatus in status)
            {
                StatusType statusType = unitStatus.status;
                StatusUIValue statusUIValue = statusUIData.GetValueByKey(statusType);

                if (statusUIValue == null)
                {
                    Debug.LogError($"No ui found for {statusType}");
                    continue;
                }

                tooltipDatas.Add(new TooltipData(TooltipData.Type.STATE, statusUIValue.title, statusUIValue.description, statusUIValue.icon));
            }

            return tooltipDatas.ToArray();
        }

        public static TooltipData[] UnitStateToTooltipData(CardData card)
        {
            if (cardUIData == null)
            {
                cardUIData = Resources.Load<CardUIData>(cardUIDataPath);
                if (cardUIData == null)
                {
                    Debug.LogError($"The status ui data is untraceable is normally at this path : Assets/Ressources/{cardUIDataPath}");
                    return null;
                }
            }

            if (card == null)
            {
                Debug.LogError($"The card send is null : {card}");
                return null;
            }

            List<TooltipData> tooltipDatas = new List<TooltipData>();

            if (card.needSacrifice)
                tooltipDatas.Add(new TooltipData(TooltipData.Type.CARD, "Sacrifice", cardUIData.NeedSacrificeDesc, null));

            if (card.isEtheral)
                tooltipDatas.Add(new TooltipData(TooltipData.Type.CARD, "Etheral", cardUIData.EtheralDesc, null));

            if (card.spells == null || card.spells.Length <= 0)
                return tooltipDatas.ToArray();

            bool alreadyAddInitiativeDesc = false;
            foreach (Spell cardSpell in card.spells)
            {
                if (cardSpell == null)
                    continue;

                SpellType spellType = cardSpell.type;
                CardUIValue cardUIValue = cardUIData.GetValueByKey(spellType);

                if (cardUIValue == null)
                {
                    Debug.LogError($"No ui found for {cardUIValue}");
                    continue;
                }

                if (cardSpell.initiative && !alreadyAddInitiativeDesc)
                {
                    tooltipDatas.Add(new TooltipData(TooltipData.Type.CARD, "Initiative", cardUIData.InitiativeDesc, null));
                    alreadyAddInitiativeDesc = true;
                }

                tooltipDatas.Add(new TooltipData(TooltipData.Type.CARD, cardUIValue.title, cardUIValue.description, null));
            }

            return tooltipDatas.ToArray();
        }
    }
}