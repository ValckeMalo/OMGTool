using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/Cards", order = 3, fileName = "NewCard")]
public class CardData : ScriptableObject
{
    [System.Serializable]
    public class Spell
    {
        public bool isInitiative = false;
        public bool showDescOnCard = true;

        public Spells spellType = Spells.Poison;
        public int amount = 0;

        public CardData cardToSpawn = null;
    }

    public CardType cardType;
    public CardType specialCardType;
    public Target target;

    public List<Spell> spells = new List<Spell>();

    public bool isBoostable = true;
    public bool needSacrifice = false;
    public bool isEtheral = false;

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
    BoostSingle,
    BoostMultiple,
    Neutral,
    Divine,
    Curse,
    Finisher,
}