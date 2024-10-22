using UnityEditor;
using UnityEngine;

namespace MaloProduction
{
    public partial class CardBuilder : EditorWindow
    {
        private void PreviewCard(CardData card, int width)
        {
            if (cardSettings == null)
            {
                LoadAssets();
            }

            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.ExpandHeight(true), GUILayout.Width(width)))
            {
                GUILayout.FlexibleSpace();
            }

            Rect scopeRect = GUILayoutUtility.GetLastRect();
            Vector2 centerScope = scopeRect.center;

            CardOptions.CardTypeTexture cardTypeTexture = cardSettings.cardsTypeTexture[(int)card.cardType];
            if (cardTypeTexture.background == null || cardSettings.wakfu == null)
            {
                return;
            }
            Texture2D backgroundTexture = cardTypeTexture.background.texture;
            if (backgroundTexture == null)
            {
                Debug.LogWarning("No background for" + card.cardType.ToString() + " Card");
                return;
            }

            ////background Card////////////////////////////////////////////////////////////////////////////////////////////
            float ratioScale = scopeRect.height / backgroundTexture.height;
            Vector2 backgroundTextureSize = new Vector2(backgroundTexture.width * ratioScale, backgroundTexture.height * ratioScale);
            Vector2 backgroundTexturePosition = new Vector2(centerScope.x - (backgroundTextureSize.x / 2), centerScope.y - (backgroundTextureSize.y / 2));
            Rect backgroundTextureRect = new Rect(backgroundTexturePosition, backgroundTextureSize);
            GUI.DrawTexture(backgroundTextureRect, backgroundTexture);

            Vector2 borderBackgroundCard = new Vector2(8f * ratioScale, 8f * ratioScale);
            Vector2 topRightBackground = new Vector2(backgroundTexturePosition.x + backgroundTextureSize.x, backgroundTexturePosition.y);
            Vector2 topLeftBackground = backgroundTexturePosition;
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

            ////Icon Card//////////////////////////////////////////////////////////////////////////////////////////////////
            Vector2 iconCardSize = new Vector2(backgroundTextureSize.x * 0.8f, backgroundTextureSize.y / 2);
            Vector2 iconCardCenter = new Vector2(centerScope.x - (iconCardSize.x / 2), centerScope.y - (iconCardSize.y / 2));
            Vector2 iconCardPosition = new Vector2(iconCardCenter.x, iconCardCenter.y - backgroundTextureSize.y * (5f / 100f));
            Rect iconCardRect = new Rect(iconCardPosition, iconCardSize);
            if (card.iconCard != null && card.iconCard.texture != null)
            {
                GUI.DrawTexture(iconCardRect, card.iconCard.texture);
            }
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

            ////wakfu icon/////////////////////////////////////////////////////////////////////////////////////////////////
            Texture2D iconWakfu = cardSettings.wakfu.texture;
            Vector2 iconWakfuSize = new Vector2(backgroundTextureSize.x / 4, backgroundTextureSize.x / 4);
            Vector2 iconWakfuPosition = new Vector2(topRightBackground.x - iconWakfuSize.x - borderBackgroundCard.x, topRightBackground.y + borderBackgroundCard.y);
            Rect iconWakfuRect = new Rect(iconWakfuPosition, iconWakfuSize);
            if (iconWakfu != null)
            {
                GUI.DrawTexture(iconWakfuRect, iconWakfu);
            }
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GUI.contentColor = Color.black;
            int pixelPerLetter = (int)(18 * ratioScale * 0.6f);
            int FontSizePreviewCard = (int)(20 * ratioScale * 0.7f);

            ////Value Card/////////////////////////////////////////////////////////////////////////////////////////////////
            Vector2 valueLabelSize = new Vector2(card.cardValue.ToString().Length * pixelPerLetter, iconWakfuSize.y);
            Vector2 valueLabelPosisiton = topLeftBackground + borderBackgroundCard;
            Rect valueLabelRect = new Rect(valueLabelPosisiton, valueLabelSize);
            GUI.Label(valueLabelRect, card.cardValue.ToString(), new GUIStyle(EditorStyles.label) { fontSize = (int)(FontSizePreviewCard / 1.5f), alignment = TextAnchor.MiddleLeft });
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

            ////wakfu Cost/////////////////////////////////////////////////////////////////////////////////////////////////
            string signWakfuCostLabel = card.wakfuCost >= 0 ? "+ " : "- ";
            string stringWakfuCost = signWakfuCostLabel + Mathf.Abs(card.wakfuCost).ToString();
            Vector2 costWakfuLabelSize = new Vector2(stringWakfuCost.Length * (int)(10 * ratioScale * 0.6f), iconWakfuSize.y);
            Vector2 costWakfuLabelPosition = new Vector2(iconWakfuPosition.x - costWakfuLabelSize.x, iconWakfuPosition.y);
            Rect wakfuCostLabelRect = new Rect(costWakfuLabelPosition, costWakfuLabelSize);
            GUI.Label(wakfuCostLabelRect, stringWakfuCost, new GUIStyle(EditorStyles.label) { fontSize = (int)(FontSizePreviewCard / 1.5f), alignment = TextAnchor.MiddleLeft });
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

            ////Name Card//////////////////////////////////////////////////////////////////////////////////////////////////
            Vector2 titleCardSize = new Vector2(backgroundTextureSize.x - borderBackgroundCard.x * 2, FontSizePreviewCard);
            Vector2 titleCardPosition = new Vector2(topLeftBackground.x + borderBackgroundCard.x, iconCardPosition.y + iconCardSize.y);
            Rect titleCardRect = new Rect(titleCardPosition, titleCardSize);
            GUI.Label(titleCardRect, card.cardName, new GUIStyle(EditorStyles.boldLabel) { fontSize = FontSizePreviewCard, alignment = TextAnchor.UpperCenter });
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

            ////Target/////////////////////////////////////////////////////////////////////////////////////////////////////
            const string target = "Cible : ";
            Vector2 targetCardSize = new Vector2(backgroundTextureSize.x - borderBackgroundCard.x * 2, (int)(FontSizePreviewCard / 1.5f));
            Vector2 targetCardPosition = new Vector2(titleCardPosition.x, titleCardPosition.y + titleCardSize.y);
            Rect targetCardRect = new Rect(targetCardPosition, targetCardSize);
            string finalTargetLabel = target + TargetStringProvider.TargetDescriptions[(int)card.target];
            GUI.Label(targetCardRect, finalTargetLabel, new GUIStyle(EditorStyles.label) { fontSize = FontSizePreviewCard / 2, alignment = TextAnchor.UpperCenter });
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

            ////Spells/////////////////////////////////////////////////////////////////////////////////////////////////////
            if (card.spells != null)
            {
                int spellsCount = card.spells.Count;
                if (spellsCount > 0)
                {
                    Vector2 spellCardSize = new Vector2(targetCardSize.x, (int)(FontSizePreviewCard / 2f));
                    Vector2 spellCardPosition = new Vector2(targetCardPosition.x, targetCardPosition.y + targetCardSize.y);
                    Rect spellCardRect = new Rect(spellCardPosition, spellCardSize);
                    string spellCardText = string.Empty;

                    for (int i = 0; i < spellsCount; i++)
                    {
                        CardData.SpellsBonus spellBonus = card.spells[i];

                        if (spellBonus.initiative)
                        {
                            spellCardText += "Initiative : ";
                            spellCardText += spellBonus.amount >= 0 ? "+ " : "- ";
                            spellCardText += Mathf.Abs(spellBonus.amount).ToString() + " " + spellBonus.spell.ToString();
                        }
                        else
                        {
                            spellCardText += spellBonus.spell.ToString() + " : ";
                            spellCardText += spellBonus.amount >= 0 ? "+ " : "- ";
                            spellCardText += Mathf.Abs(spellBonus.amount).ToString();
                        }

                        GUI.Label(spellCardRect, spellCardText, new GUIStyle(EditorStyles.label) { fontSize = FontSizePreviewCard / 2, alignment = TextAnchor.UpperCenter });

                        spellCardText = string.Empty;
                        spellCardRect.y += spellCardSize.y;
                    }
                }
            }
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GUI.contentColor = Color.white;

            ////Value icon/////////////////////////////////////////////////////////////////////////////////////////////////
            if (cardTypeTexture.iconCard != null)
            {
                Texture2D valueIcon = cardTypeTexture.iconCard.texture;
                Vector2 valueIconSize = new Vector2(backgroundTextureSize.x / 4, backgroundTextureSize.x / 4);
                Vector2 valueIconPosition = new Vector2(valueLabelPosisiton.x + valueLabelSize.x, valueLabelPosisiton.y);
                Rect valueIconRect = new Rect(valueIconPosition, valueIconSize);
                if (valueIcon != null)
                {
                    GUI.DrawTexture(valueIconRect, valueIcon);
                }
            }
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }
    }
}