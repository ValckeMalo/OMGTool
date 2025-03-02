namespace OMG.Data.Card
{
    using UnityEngine;
    using System.Collections.Generic;

    [CreateAssetMenu(fileName = "CardLibrary", order = 1, menuName = "Card/Library")]
    public class CardLibrary : ScriptableObject
    {
        public List<CardData> cards = new List<CardData>();
    }
}