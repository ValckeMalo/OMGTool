namespace OMG.Unit.Action
{
    using OMG.Unit;
    using OMG.Unit.Status;

    public abstract class UnitAction
    {
        public abstract void Execute(IUnit player, IUnit monster, IUnit[] monsters);

        protected void Attack(int damage, IUnit unit)
        {
            unit.Damage(damage);
        }
        protected void Attack(int damage, params IUnit[] units)
        {
            foreach (IUnit unit in units)
            {
                unit.Damage(damage);
            }
        }

        protected void PiecringAttack(int damage, IUnit unit)
        {
            unit.PiercingDamage(damage);
        }
        protected void PiercingAttack(int damage, params IUnit[] units)
        {
            foreach (IUnit unit in units)
            {
                unit.PiercingDamage(damage);
            }
        }

        protected void Heal(int heal, IUnit unit)
        {
            unit.Heal(heal);
        }
        protected void Heal(int heal, params IUnit[] units)
        {
            foreach (IUnit unit in units)
            {
                unit.Heal(heal);
            }
        }

        protected void AddArmor(int armor, IUnit unit)
        {
            unit.AddArmor(armor);
        }
        protected void AddArmor(int armor, params IUnit[] units)
        {
            foreach (IUnit unit in units)
            {
                unit.AddArmor(armor);
            }
        }

        protected void ClearArmor(IUnit unit)
        {
            unit.ClearArmor();
        }
        protected void ClearArmor(IUnit[] units)
        {
            foreach (IUnit unit in units)
            {
                unit.ClearArmor();
            }
        }

        protected void AddStatus(int nbTurn, StatusType status, IUnit unit)
        {
            unit.AddStatus(status, nbTurn);
        }
        protected void AddStatus(int nbTurn, StatusType status, params IUnit[] units)
        {
            foreach (IUnit unit in units)
            {
                unit.AddStatus(status, nbTurn);
            }
        }

        protected void ClearStatus(StatusType status, IUnit unit)
        {
            unit.ClearStatus(status);
        }
        protected void ClearStatus(StatusType status, params IUnit[] units)
        {
            foreach (IUnit unit in units)
            {
                unit.ClearStatus(status);
            }
        }

        protected void ClearAllStatus(IUnit unit)
        {
            unit.ClearAllStatus();
        }
        protected void ClearAllStatus(params IUnit[] units)
        {
            foreach (IUnit unit in units)
            {
                unit.ClearAllStatus();
            }
        }
    }
}