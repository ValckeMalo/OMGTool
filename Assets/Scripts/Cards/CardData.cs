using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeckBuilder/Cards", order = 0, fileName = "NewCard")]
public class CardData : ScriptableObject
{
    [System.Serializable]
    public class SpellsBonus
    {
        public bool initiative;
        public Spells spell;
        public int amount;
    }

    public CardType cardType;
    public Target target;

    public List<SpellsBonus> spells;

    public int wakfuCost;
    public int cardValue;

    public string cardName;

    public Texture2D iconCard;
}

public static class PPPP
{
    public static string[] targetString = new string[(int)Target.Count] { "premier ennemi", "dernier ennemi", "tous les ennemis", "Oropo", };
}

public enum Spells
{
    Poison,
    Plaie,
    Shield,
    Tenacite,
    Sacrifice,
    Bousculade,
    Eveil,
}

public enum Target
{
    FirstEnemy,
    LastEnemy,
    AllEnemy,
    Me,
    Count,
}

public enum CardType
{
    Attack,
    Defense,
    Boost,
    Neutral,
    GodPositive,
    GodNegativ,
    Finisher,
}