namespace OMG.Card.Data
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Card/Cards", order = 3, fileName = "NewCard")]
    public class CardData : ScriptableObject
    {
        public CardBackground background;
        public CardAction type;

        public int value = 0;
        [Range(-6, 6)] public int wakfu = 0;
        public new string name = string.Empty;

        public Sprite icon = null;

        public bool isBoostable = true;
        public bool isEtheral = false;
        public bool needSacrifice = false;

        public Target target;

        public Spell[] spells = null;
    }

    public enum CardBackground
    {
        Attack,
        Defense,
        Boost,
        Neutral,
        Divine,
        Curse,
        Finisher,
    }
    public enum CardAction
    {
        Attack,
        Defense,
        Boost,
        Neutral,
    }
    public enum Target
    {
        None,
        Oropo,
        FirstMonster,
        LastMonster,
        AllMonsters,
        RandomUnit,
        OneCard,
        AllCards,
        RandomCard,
    }

    public static class TargetStringProvider
    {
        public static string[] TargetDescriptions =
        {
            "",
            "Oropo",
            "First Monster",
            "Last Monster",
            "All Monsters",
            "Random Unit",
            "Boost One Card",
            "Boost All Cards",
            "Boost a Random Card",
        };
    }
}