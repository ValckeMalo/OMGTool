namespace OMG.Game.Tooltip
{
    using UnityEngine;

    public class TooltipData
    {
        private TooltipType type = TooltipType.CARD;
        private string header = string.Empty;
        private string body = string.Empty;
        private Sprite icon = null;

        public TooltipType Type => type;
        public string Header => header;
        public string Body => body;
        public Sprite Icon => icon;

        public TooltipData(TooltipType type, string header, string body, Sprite icon)
        {
            this.type = type;
            this.header = header;
            this.body = body;
            this.icon = icon;
        }
    }
}