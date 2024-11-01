namespace OMG.Unit
{
    public abstract class Unit : IUnit
    {
        public abstract string GetName();

        #region IUnit
        public void AddArmor(int armor)
        {
            throw new System.NotImplementedException();
        }
        public void AddStatus(Status status, int nbTurn)
        {
            throw new System.NotImplementedException();
        }
        public void ClearAllStatus()
        {
            throw new System.NotImplementedException();
        }
        public void ClearArmor()
        {
            throw new System.NotImplementedException();
        }
        public void ClearStatus(Status status)
        {
            throw new System.NotImplementedException();
        }
        public void Damage(int damage)
        {
            throw new System.NotImplementedException();
        }
        public void Heal(int life)
        {
            throw new System.NotImplementedException();
        }
        public void PiercingDamage(int damage)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}