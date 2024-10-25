using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CardLibrary", order = 1, menuName = "CardBuilder/CardLibrary")]
public class CardLibrary : ScriptableObject
{
    public List<CardData> cards = new List<CardData>();
}