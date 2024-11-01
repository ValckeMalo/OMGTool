namespace OMG.Unit
{
    public interface IUnit
    {
        public void Damage(int damage);
        public void PiercingDamage(int damage);
        public void Heal(int life);
        public void AddArmor(int armor);
        public void ClearArmor();
        public void AddStatus(Status status, int nbTurn);
        public void ClearStatus(Status status);
        public void ClearAllStatus();
    }

    public enum Status
    {
        Poison,
        Plaie,
        Shield,
        Tenacite,
        Sacrifice,
        Bousculade,
        Eveil,
    }
}