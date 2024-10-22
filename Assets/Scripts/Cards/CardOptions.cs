using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CardOptions",order = 2,menuName = "DeckBuilder/CardOptions")]
public class CardOptions : ScriptableObject
{
    public List<CardTypeTexture> cardsTypeTexture = new List<CardTypeTexture>();

    public Sprite wakfu;

    [System.Serializable]
    public class CardTypeTexture
    {
        public CardType type;
        public Sprite background;
        public Sprite iconCard;
    }
}