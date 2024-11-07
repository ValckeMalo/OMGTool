namespace OMG.Unit
{
    using System.Linq;
    using UnityEngine;
    using OMG.Unit.Status;

    public abstract class Unit : ScriptableObject, IUnit
    {
        [SerializeField] protected UnitData unitData;

        public abstract string GetName();

        #region IUnit
        public virtual void AddArmor(int armor)
        {
            unitData.armor += armor;
        }
        public virtual void AddStatus(StatusType statusType, int nbTurn)
        {
            UnitStatus status = unitData.status.Where(x => x.status == statusType).FirstOrDefault();
            if (status != null)
            {
                status.turn += nbTurn;
            }
            else
            {
                unitData.status.Add(new UnitStatus(statusType, nbTurn));
            }
        }
        public virtual void ClearAllStatus()
        {
            unitData.status.Clear();
        }
        public virtual void ClearArmor()
        {
            unitData.armor = 0;
        }
        public virtual void ClearStatus(StatusType statusType)
        {
            UnitStatus status = unitData.status.Where(x => x.status == statusType).FirstOrDefault();
            if (status != null)
            {
                unitData.status.Remove(status);
            }
        }
        public virtual void Damage(int damage)
        {
            int damageToDeal = unitData.armor - damage;

            if (damageToDeal < 0)
            {
                unitData.hp += damageToDeal;
                unitData.armor = 0;
            }
            else
            {
                unitData.armor -= damage;
            }
        }
        public virtual void Heal(int life)
        {
            unitData.hp = Mathf.Min(unitData.hp + life, unitData.maxHp);
        }
        public virtual void PiercingDamage(int damage)
        {
            unitData.hp -= damage;
        }
        #endregion
    }
}