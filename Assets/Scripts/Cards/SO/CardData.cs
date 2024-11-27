using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/Cards", order = 3, fileName = "NewCard")]
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

    public List<SpellsBonus> spells = new List<SpellsBonus>();

    public int wakfuCost;
    public int cardValue;

    public string cardName;

    public Sprite iconCard;
}

public static class TargetStringProvider
{
    /// <summary>
    /// Array of strings representing different types of targets.
    /// </summary>
    public static string[] TargetDescriptions = new string[(int)Target.Count]
    {
        "first monster",
        "last monster",
        "all monsters",
        "Oropo"
    };
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
    Player,
    Count,
}

public enum CardType
{
    Attack,
    Defense,
    Boost,
    Neutral,
    Divine,
    Curse,
    Finisher,
}