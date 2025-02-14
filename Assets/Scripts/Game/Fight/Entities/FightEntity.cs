namespace OMG.Game.Fight.Entities
{
    using OMG.Unit.Status;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public abstract class FightEntity
    {
        protected int currentHealth;
        private int maxHealth;
        protected int currentArmor;
        protected List<UnitStatus> currentStatus;
        protected FightEntityUI entityUI;

        public float EntityPercentHealth => currentHealth / maxHealth;
        public int EntityCurrentHealth => currentHealth;
        public int Currentarmor => currentArmor;

        protected void InitializeEntity(int currentHealth, int maxHealth, FightEntityUI entityUI)
        {
            this.currentHealth = currentHealth;
            this.maxHealth = maxHealth;
            currentArmor = 0;
            currentStatus = new List<UnitStatus>();
            this.entityUI = entityUI;
        }
        public virtual void NewTurn()
        {
            if (!HasStatus(StatusType.Tenacite)) EraseArmor();

            currentStatus.ForEach((unitStatus) => UpdateStatus(unitStatus));
        }
        public abstract void EndTurn();

        public void LoseHealth(int damage, bool isPiercingDamage = false)
        {
            if (HasStatus(StatusType.Invincibility)) return;

            if (!isPiercingDamage) //damage the armor
            {
                if (currentArmor < damage)
                {
                    damage -= currentArmor;
                    currentArmor = 0;
                }
                else
                {
                    currentArmor -= damage;
                    return;
                }
            }

            currentHealth -= damage;


        }
        public void Heal(int valueHeal)
        {
            currentHealth = Mathf.Min(currentHealth + valueHeal, maxHealth);
        }

        public void EraseArmor()
        {
            currentArmor = 0;
        }
        public void AddArmor(int armorToAdd)
        {
            currentArmor += armorToAdd;
        }

        public void AddStatus(StatusType statusType, int value)
        {
            UnitStatus statusSelect = currentStatus.Find(status => status.status == statusType);
            if (statusSelect != null)
            {
                statusSelect.turn += value;
            }
            else
            {
                currentStatus.Add(new UnitStatus(statusType, value));
            }
        }
        public void RemoveStatus(StatusType statusType)
        {
            UnitStatus statusSelect = currentStatus.Find(status => status.status == statusType);
            if (statusSelect != null)
            {
                currentStatus.Remove(statusSelect);
            }
        }
        public bool HasStatus(StatusType statusType)
        {
            return currentStatus.Find(status => status.status == statusType) != null;
        }
        public int GetValueOfStatus(StatusType statusType)
        {
            UnitStatus selectStatus = currentStatus.Find(status => status.status == statusType);
            return selectStatus == null ? -1 : selectStatus.turn;
        }
        private void UpdateStatus(UnitStatus unitStatus)
        {
            switch (unitStatus.status)
            {
                case StatusType.Poison:
                    LoseHealth(unitStatus.turn);
                    break;
            }

            unitStatus.turn -= 1;
        }
    }
}