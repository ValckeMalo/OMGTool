namespace OMG.Data.Card
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Card/Cards", order = 3, fileName = "NewCard")]
    public class CardData : ScriptableObject
    {
        public CardBackground background;
        public CardAction type;

        public int value = 0;
        [Range(-6, 6)] public int energy = 0;
        public new string name = string.Empty;

        public Sprite icon = null;

        public bool isBoostable = true;
        public bool isEtheral = false;
        public bool needSacrifice = false;

        public Target target;

        public CardSpell[] spells = null;
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
        RandomEntity,
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
            "First Mobs",
            "Last Mobs",
            "All Mobs",
            "Random Entity",
            "Boost One Card",
            "Boost All Cards",
            "Boost a Random Card",
        };
    }
}