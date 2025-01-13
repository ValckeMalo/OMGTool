namespace OMG.Unit.Action
{
    using MaloProduction.CustomAttributes;

    using OMG.Unit;
    using OMG.Unit.Status;

    public abstract class UnitAction : UnityEngine.ScriptableObject
    {
        [Title("Unit Action")]
        [UnityEngine.SerializeField] protected string Description = string.Empty;
        [UnityEngine.SerializeField] protected int Value = 0;

        public abstract void Execute(IUnit unit);
        public abstract void Execute(IUnit[] units);

        public virtual int GetValue()
        {
            return Value;
        }
        public virtual string GetDescription()
        {
            return Description;
        }

        protected void Attack(int damage, IUnit unit)
        {
            unit.Damage(damage);
        }
        protected void Attack(int damage, IUnit[] units)
        {
            foreach (IUnit unit in units)
            {
                unit.Damage(damage);
            }
        }

        protected void PiercingAttack(int damage, IUnit unit)
        {
            unit.PiercingDamage(damage);
        }
        protected void PiercingAttack(int damage, IUnit[] units)
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
        protected void Heal(int heal, IUnit[] units)
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
        protected void AddArmor(int armor, IUnit[] units)
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
        protected void AddStatus(int nbTurn, StatusType status, IUnit[] units)
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
        protected void ClearStatus(StatusType status, IUnit[] units)
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
        protected void ClearAllStatus(IUnit[] units)
        {
            foreach (IUnit unit in units)
            {
                unit.ClearAllStatus();
            }
        }
    }
}