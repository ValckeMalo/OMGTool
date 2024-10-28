using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewCardDeck", menuName = "Card/Deck", order = 0)]
public class CardDeck : ScriptableObject
{
    public List<CardData> cards = new List<CardData>();
}