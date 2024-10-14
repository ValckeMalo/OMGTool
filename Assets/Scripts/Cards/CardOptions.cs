using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CardOptions",order = 2,menuName = "DeckBuilder/CardOptions")]
public class CardOptions : ScriptableObject
{
    public List<CardBgTexture> collectionBgCardTexture = new List<CardBgTexture>();

    public Texture2D wakfu;
    public Texture2D attack;

    [System.Serializable]
    public class CardBgTexture
    {
        public CardType type;
        public Texture2D bgCardTexture;
    }
}