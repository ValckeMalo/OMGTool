namespace OMG.Tools.PreviewCard
{
    using UnityEditor;
    using UnityEngine;
    using OMG.Data.Card;

    public static class PreviewCardEditor
    {
        private const int BaseValueFontSize = 19;
        private const int BaseTitleFontSize = 22;
        private const int BaseSpellFontSize = 16;
        private const int BaseTargetFontSize = 12;

        private static GUIStyle ValueStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = BaseValueFontSize };
        private static GUIStyle TitleStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = BaseTitleFontSize };
        private static GUIStyle SpellsStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = BaseSpellFontSize };
        private static GUIStyle TargetStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = BaseTargetFontSize };

        private const float BaseMargin = 10f;
        private static float margin = BaseMargin;

        private const float topPercent = 0.1f;
        private const float centerpPercent = 0.6f;
        private const float bottompPercent = 0.3f;

        public static void PreviewCard(CardData cardData, CardSettings settings, float width)
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

            Sprite backgroundSprite = settings.backgroundSprite[(int)cardData.background].sprite;
            Sprite iconType = settings.iconSprite[(int)cardData.type].sprite;

            if (backgroundSprite == null)
            {
                LogError($"No background for : {cardData.background}.");
                return;
            }
            if (iconType == null)
            {
                LogError($"No Icon card for : {cardData.type}.");
            }
            if (settings.energy == null)
            {
                LogError($"No Wakfu Sprite inside the settings");
            }

            Texture2D backgroundTexture = backgroundSprite.texture;

            using (EditorGUILayout.HorizontalScope scope = new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandHeight(true), GUILayout.Width(width)))
            {
                GUILayout.FlexibleSpace();

                Rect scopeRect = scope.rect;
                float ratioScale = scopeRect.height / backgroundTexture.height;

                UpdateFontSize(ratioScale);

                GUI.contentColor = Color.black;

                Rect backgroundRect = Background(backgroundTexture, scopeRect, ratioScale);
                Rect topRect = TopCard(cardData.value, (iconType != null) ? iconType.texture : null,
                                       cardData.energy, (settings.energy != null) ? settings.energy.texture : null, backgroundRect);
                Rect centerRect = CenterCard((cardData.icon != null) ? cardData.icon.texture : null, topRect, backgroundRect);
                BottomCard(cardData, centerRect, backgroundRect);

                GUI.contentColor = Color.white;
            }
        }

        private static void UpdateFontSize(float ratioScale)
        {
            ValueStyle.fontSize = (int)(BaseValueFontSize * ratioScale);
            TitleStyle.fontSize = (int)(BaseTitleFontSize * ratioScale);
            SpellsStyle.fontSize = (int)(BaseSpellFontSize * ratioScale);
            TargetStyle.fontSize = (int)(BaseSpellFontSize * ratioScale);
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
        private static void BottomCard(CardData card, Rect centerRect, Rect backgroundRect)
        {
            Vector2 bottomSize = new Vector2(backgroundRect.width, backgroundRect.height * bottompPercent);
            Vector2 bottomPosition = new Vector2(backgroundRect.x, centerRect.y + centerRect.height);
            Rect bottomRect = new Rect(bottomPosition, bottomSize);

            //TITLE
            GUIContent titleContent = new GUIContent(card.name);
            Vector2 titleSize = new Vector2(bottomSize.x, TitleStyle.CalcHeight(titleContent, bottomSize.x));
            Rect titleRect = new Rect(bottomPosition, titleSize);
            GUI.Label(titleRect, titleContent, TitleStyle);

            //TARGET
            GUIContent targetContent = new GUIContent("Target : " + TargetStringProvider.TargetDescriptions[(int)card.target]);

            //Spell
            Vector2 targetSize = new Vector2(bottomSize.x, TargetStyle.CalcHeight(titleContent, bottomSize.x));
            Vector2 targetPosition = new Vector2(titleRect.x, titleRect.y + titleRect.height);
            Rect targetRect = new Rect(targetPosition, targetSize);
            GUI.Label(targetRect, targetContent, TargetStyle);

            Rect spellsRect = Spells(card.spells, targetRect);

            //Sacrifice
            if (card.needSacrifice)
            {
                Rect startRect = (spellsRect == Rect.zero) ? targetRect : spellsRect;

                GUIContent sacrificeContent = new GUIContent("SACRIFICE");
                Vector2 sacrificeSize = new Vector2(bottomSize.x, TargetStyle.CalcHeight(sacrificeContent, bottomSize.x));
                Vector2 sacrificePosition = new Vector2(startRect.x, startRect.y + startRect.height);
                Rect sacrificeRect = new Rect(sacrificePosition, sacrificeSize);
                GUI.Label(sacrificeRect, sacrificeContent, TargetStyle);
            }
        }
        private static Rect Spells(CardSpell[] spells, Rect targetRect)
        {
            if (spells != null)
            {
                int spellsCount = spells.Length;
                if (spellsCount > 0)
                {
                    string spellsText = string.Empty;

                    foreach (CardSpell spellBonus in spells)
                    {
                        if (spellBonus != null && spellBonus.shownOnCard)
                        {
                            if (spellBonus.initiative)
                            {
                                spellsText += "Initiative : ";
                                spellsText += spellBonus.value >= 0 ? "+ " : "- ";
                                spellsText += Mathf.Abs(spellBonus.value).ToString() + " " + spellBonus.type.ToString();
                            }
                            else
                            {
                                spellsText += spellBonus.type.ToString() + " : ";
                                spellsText += spellBonus.value >= 0 ? "+ " : "- ";
                                spellsText += Mathf.Abs(spellBonus.value).ToString();
                            }

                            spellsText += "\n";
                        }
                    }

                    GUIContent spellContent = new GUIContent(spellsText);
                    float heightSpells = (spellsText == string.Empty) ? 0f : SpellsStyle.CalcHeight(spellContent, targetRect.width);

                    Vector2 spellsSize = new Vector2(targetRect.width, heightSpells);
                    Vector2 spellsPosition = new Vector2(targetRect.x, targetRect.y + targetRect.height);
                    Rect spellsRect = new Rect(spellsPosition, spellsSize);

                    GUI.Label(spellsRect, spellContent, SpellsStyle);

                    return spellsRect;
                }
            }

            return Rect.zero;
        }

        private static void LogError(string message, bool isShow = false)
        {
            if (isShow)
            {
                Debug.LogError(message);
            }
        }
    }
}