namespace OMG.Unit
{
    using OMG.Unit.Status;

    public interface IUnit
    {
        public void Damage(int damage);
        public void PiercingDamage(int damage);
        public void Heal(int life);
        public void AddArmor(int armor);
        public void ClearArmor();
        public void AddStatus(StatusType status, int nbTurn);
        public void ClearStatus(StatusType status);
        public void ClearAllStatus();

        public void UpdateUnit();
    }
}