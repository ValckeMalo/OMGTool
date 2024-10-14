using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CardLibrary", order = 1, menuName = "DeckBuilder/CardLibrary")]
public class CardLibrary : ScriptableObject
{
    public List<CardData> cardsLibrary;
}