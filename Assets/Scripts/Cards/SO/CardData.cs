using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/Cards", order = 3, fileName = "NewCard")]
public class CardData : ScriptableObject
{
    [System.Serializable]
    public class Spell
    {
        public bool isInitiative;
        public Spells spellType;
        public int amount;
    }

    public CardType cardType;
    public Target target;

    public List<Spell> spells = new List<Spell>();
    public bool needSacrifice = false;

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
    Bousculade,
    Eveil,
}

public enum Target
{
    FirstMonster,
    LastMonster,
    AllMonsters,
    Oropo,
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