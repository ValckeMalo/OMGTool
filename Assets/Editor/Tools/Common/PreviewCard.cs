namespace OMG.Tools.PreviewCard
{
    using UnityEditor;
    using UnityEngine;
    using System.Collections.Generic;
    using static CardOptions;

    public static class PreviewCardEditor
    {
        private const int BaseValueFontSize = 19;
        private const int BaseTitleFontSize = 22;
        private const int BaseSpellFontSize = 16;

        private static GUIStyle ValueStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = BaseValueFontSize };
        private static GUIStyle TitleStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = BaseTitleFontSize };
        private static GUIStyle SpellsStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = BaseSpellFontSize };

        private const float BaseMargin = 10f;
        private static float margin = BaseMargin;

        private const float topPercent = 0.1f;
        private const float centerpPercent = 0.6f;
        private const float bottompPercent = 0.3f;

        public static void PreviewCard(CardData cardData, CardOptions settings, float width)
        {
            if (cardData == null)
            {
                LogError($"The data of the card give is null");
                return;
            }
            if (settings == null)
            {
                LogError($"The options gives is null");
                return;
            }
            CardTypeTexture cardTexture = settings.cardsTypeTexture[(int)cardData.cardType];
            if (cardTexture.background == null)
            {
                LogError($"No background for : {cardData.cardType}.");
                return;
            }
            if (cardTexture.iconCard == null)
            {
                LogError($"No Icon card for : {cardData.cardType}.");
            }
            if (settings.wakfu == null)
            {
                LogError($"No Wakfu Sprite inside the settings");
            }

            Texture2D backgroundTexture = cardTexture.background.texture;

            using (EditorGUILayout.HorizontalScope scope = new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandHeight(true), GUILayout.Width(width)))
            {
                GUILayout.FlexibleSpace();

                Rect scopeRect = scope.rect;
                float ratioScale = scopeRect.height / backgroundTexture.height;

                UpdateFontSize(ratioScale);

                GUI.contentColor = Color.black;

                Rect backgroundRect = Background(backgroundTexture, scopeRect, ratioScale);
                Rect topRect = TopCard(cardData.cardValue, (cardTexture.iconCard != null) ? cardTexture.iconCard.texture : null,
                                       cardData.wakfuCost, (settings.wakfu != null) ? settings.wakfu.texture : null, backgroundRect);
                Rect centerRect = CenterCard((cardData.iconCard != null) ? cardData.iconCard.texture : null, topRect, backgroundRect);
                BottomCard(cardData.cardName, cardData.spells, centerRect, backgroundRect);

                GUI.contentColor = Color.white;
            }
        }

        private static void UpdateFontSize(float ratioScale)
        {
            ValueStyle.fontSize = (int)(BaseValueFontSize * ratioScale);
            TitleStyle.fontSize = (int)(BaseTitleFontSize * ratioScale);
            SpellsStyle.fontSize = (int)(BaseSpellFontSize * ratioScale);
            margin = BaseMargin * ratioScale;
        }

        private static Rect Background(Texture2D backgroundTexture, Rect scopeRect, float ratioScale)
        {
            if (backgroundTexture == null)
            {
                LogError($"The Background texture for the preview card doesn't exist.");
                return Rect.zero;
            }

            Vector2 backgroundSize = new Vector2(backgroundTexture.width * ratioScale, backgroundTexture.height * ratioScale);
            Vector2 backgroundPosition = new Vector2(scopeRect.center.x - backgroundSize.x / 2f, scopeRect.y);
            Rect backgroundRect = new Rect(backgroundPosition, backgroundSize);

            GUI.DrawTexture(backgroundRect, backgroundTexture);

            return backgroundRect;
        }
        private static Rect TopCard(int valueCard, Texture2D valueIcon, int wakfuValue, Texture2D wakfuIcon, Rect backgroundRect)
        {
            Vector2 topSize = new Vector2(backgroundRect.width, backgroundRect.height * topPercent);
            Vector2 topBoxSize = new Vector2(topSize.x * 0.4f, topSize.y);

            Vector2 topBoxPosition = new Vector2(backgroundRect.position.x, backgroundRect.position.y + margin);
            Rect topRect = new Rect(topBoxPosition, topSize);

            Rect topLeftRect = new Rect(new Vector2(topRect.position.x + margin, topRect.position.y), topBoxSize);
            ValueCard(valueCard, valueIcon, topLeftRect);

            Rect topRightRect = new Rect(new Vector2(topRect.position.x + topRect.width - topBoxSize.x - margin, topRect.y), topBoxSize);
            WakfuCard(wakfuValue, wakfuIcon, topRightRect);

            return topRect;
        }
        private static void WakfuCard(int wakfuValue, Texture2D wakfuIcon, Rect topRightRect)
        {
            Vector2 wakfuCardSize = new Vector2(topRightRect.width / 2f, topRightRect.height);

            Vector2 wakfuLabelPosition = topRightRect.position;
            Rect wakfuLabelRect = new Rect(wakfuLabelPosition, wakfuCardSize);

            string signWakfuCostLabel = wakfuValue >= 0 ? "+ " : "- ";
            string stringWakfuCost = signWakfuCostLabel + Mathf.Abs(wakfuValue).ToString();
            GUI.Label(wakfuLabelRect, new GUIContent(stringWakfuCost), ValueStyle);

            if (wakfuIcon == null)
            {
                Debug.Log($"The value icon doesn't exist for the preview of the card");
                return;
            }

            Vector2 wakfuTexturePosition = new Vector2(wakfuLabelPosition.x + wakfuCardSize.x, topRightRect.position.y);
            Rect wakfuTextureRect = new Rect(wakfuTexturePosition, wakfuCardSize);
            GUI.DrawTexture(wakfuTextureRect, wakfuIcon);

        }
        private static void ValueCard(int valueCard, Texture2D valueIcon, Rect topLeftRect)
        {
            Vector2 valueCardSize = new Vector2(topLeftRect.width / 2f, topLeftRect.height);

            Vector2 valueLabelPosition = topLeftRect.position;
            Rect valueLabelRect = new Rect(valueLabelPosition, valueCardSize);
            GUI.Label(valueLabelRect, new GUIContent(valueCard.ToString()), ValueStyle);

            if (valueIcon == null)
            {
                LogError($"The value icon doesn't exist for the preview of the card");
                return;
            }

            Vector2 valueTexturePosition = new Vector2(valueLabelPosition.x + valueCardSize.x, topLeftRect.position.y);
            Rect valueTextureRect = new Rect(valueTexturePosition, valueCardSize);
            GUI.DrawTexture(valueTextureRect, valueIcon);
        }
        private static Rect CenterCard(Texture2D iconCard, Rect topRect, Rect backgroundRect)
        {
            Vector2 iconCardSize = new Vector2(backgroundRect.width * 0.9f, backgroundRect.height * centerpPercent);
            Vector2 iconCardPosition = new Vector2(topRect.center.x - iconCardSize.x / 2f, topRect.y + topRect.height);
            Rect iconCardRect = new Rect(iconCardPosition, iconCardSize);

            if (iconCard == null)
            {
                LogError($"The card icon texture for the preview card doesn't exist.");
                return iconCardRect;
            }

            GUI.DrawTexture(iconCardRect, iconCard);

            return iconCardRect;
        }
        private static void BottomCard(string cardName, List<CardData.SpellsBonus> spells, Rect centerRect, Rect backgroundRect)
        {
            Vector2 bottomSize = new Vector2(backgroundRect.width, backgroundRect.height * bottompPercent);
            Vector2 bottomPosition = new Vector2(backgroundRect.x, centerRect.y + centerRect.height);
            Rect bottomRect = new Rect(bottomPosition, bottomSize);

            //TITLE
            GUIContent titleContent = new GUIContent(cardName);
            Vector2 titleSize = new Vector2(bottomSize.x, TitleStyle.CalcHeight(titleContent, bottomSize.x));
            Rect titleRect = new Rect(bottomPosition, titleSize);
            GUI.Label(titleRect, titleContent, TitleStyle);

            Spells(spells, bottomRect, titleRect);
        }
        private static void Spells(List<CardData.SpellsBonus> spells, Rect bottomRect, Rect titleRect)
        {
            if (spells != null)
            {
                int spellsCount = spells.Count;
                if (spellsCount > 0)
                {
                    Vector2 spellsSize = new Vector2(bottomRect.width, bottomRect.height - titleRect.height);
                    Vector2 spellsPosition = new Vector2(bottomRect.x, bottomRect.y + titleRect.height);
                    Rect spellsRect = new Rect(spellsPosition, spellsSize);
                    string spellsText = string.Empty;

                    foreach (CardData.SpellsBonus spellBonus in spells)
                    {
                        if (spellBonus != null)
                        {
                            if (spellBonus.initiative)
                            {
                                spellsText += "Initiative : ";
                                spellsText += spellBonus.amount >= 0 ? "+ " : "- ";
                                spellsText += Mathf.Abs(spellBonus.amount).ToString() + " " + spellBonus.spell.ToString();
                            }
                            else
                            {
                                spellsText += spellBonus.spell.ToString() + " : ";
                                spellsText += spellBonus.amount >= 0 ? "+ " : "- ";
                                spellsText += Mathf.Abs(spellBonus.amount).ToString();
                            }

                            spellsText += "\n";
                        }
                    }

                    GUIContent spellContent = new GUIContent(spellsText);
                    GUI.Label(spellsRect, spellContent, SpellsStyle);
                }
            }
        }

        private static void LogError(string message,bool isShow = false)
        {
            if (isShow)
            {
                Debug.LogError(message);
            }
        }
    }
}