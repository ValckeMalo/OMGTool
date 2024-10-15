using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CardOptions",order = 2,menuName = "DeckBuilder/CardOptions")]
public class CardOptions : ScriptableObject
{
    public List<CardTypeTexture> collectionBgCardTexture = new List<CardTypeTexture>();

    public Texture2D wakfu;

    [System.Serializable]
    public class CardTypeTexture
    {
        public CardType type;
        public Texture2D background;
        public Texture2D iconCard;
    }
}