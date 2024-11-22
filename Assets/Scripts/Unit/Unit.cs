namespace OMG.Unit
{
    using System.Linq;
    using UnityEngine;
    using OMG.Unit.Status;
    using MaloProduction.CustomAttributes;

    public abstract class Unit : ScriptableObject, IUnit
    {
        public delegate void EventUnitDataModified(UnitData unitData);
        public EventUnitDataModified OnUnitDataModified;

        [Title("Unit")]
        [SerializeField] protected UnitData unitData;
        public UnitData Data { get => unitData; }
        [SerializeField] protected string unitName;

        public virtual string GetName()
        {
            return unitName;
        }

        private void CallDataModified()
        {
            OnUnitDataModified?.Invoke(unitData);
        }

        #region IUnit
        public virtual void AddArmor(int armor)
        {
            unitData.armor += armor;
            CallDataModified();
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
            CallDataModified();
        }
        public virtual void ClearAllStatus()
        {
            unitData.status.Clear();
            CallDataModified();
        }
        public virtual void ClearArmor()
        {
            unitData.armor = 0;
            CallDataModified();
        }
        public virtual void ClearStatus(StatusType statusType)
        {
            UnitStatus status = unitData.status.Where(x => x.status == statusType).FirstOrDefault();
            if (status != null)
            {
                unitData.status.Remove(status);
            }
            CallDataModified();
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
            CallDataModified();
        }
        public virtual void Heal(int life)
        {
            unitData.hp = Mathf.Min(unitData.hp + life, unitData.maxHp);
            CallDataModified();
        }
        public virtual void PiercingDamage(int damage)
        {
            unitData.hp -= damage;
            CallDataModified();
        }
        #endregion
    }
}