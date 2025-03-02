namespace OMG.Data.Card
{
    [System.Serializable]
    public class CardSpell
    {
        public int value = 0;
        public SpellType type;

        public CardData cardToSpawn = null;

        public bool shownOnCard = true;
        public bool initiative = false;
    }

    public enum SpellType
    {
        Poison,
        Plaie,
        Shield,
        Tenacite,
        Bousculade,
        Eveil,
    }
}