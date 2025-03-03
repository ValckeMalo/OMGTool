namespace OMG.Game.Fight.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    [Serializable]
    public abstract class FightEntity
    {
        [Header("Entity UI")]
        [SerializeField] protected FightEntityUI entityUI;

        protected int currentHealth;
        private int maxHealth;
        protected int currentArmor;
        protected List<FightEntityStatus> currentStatus;

        public float EntityPercentHealth => currentHealth / maxHealth;
        public int EntityCurrentHealth => currentHealth;
        public int Currentarmor => currentArmor;
        public bool IsDying() => (currentHealth <= 0);

        protected void InitializeEntity(int currentHealth, int maxHealth, FightEntityUI entityUI)
        {
            this.currentHealth = currentHealth;
            this.maxHealth = maxHealth;
            this.entityUI = entityUI;

            currentArmor = 0;
            currentStatus = new List<FightEntityStatus>();

            entityUI.UpdateAllStatusUI(currentStatus);
            entityUI.InitHealthUI(this.currentHealth, this.maxHealth, currentArmor);
        }
        public void NewTurn()
        {
            if (!HasStatus(StatusType.Tenacite)) EraseArmor();

            currentStatus.ForEach((entityStatus) => UpdateStatus(entityStatus));

            entityUI.UpdateAllStatusUI(currentStatus);
        }

        public void LoseHealth(int damage, bool isPiercingDamage = false)
        {
            if (HasStatus(StatusType.Invincibility)) return;

            if (!isPiercingDamage)// damage the armor
            {
                if (currentArmor < damage)
                {
                    damage -= currentArmor;
                    currentArmor = 0;
                }
                else
                {
                    currentArmor -= damage;
                    entityUI.UpdateHealthUI(currentHealth, currentArmor);
                    return;
                }
            }

            currentHealth = Mathf.Max(currentHealth - damage, 0);
            entityUI.UpdateHealthUI(currentHealth, currentArmor);
        }
        public void Heal(int valueHeal)
        {
            currentHealth = Mathf.Min(currentHealth + valueHeal, maxHealth);
            entityUI.UpdateHealthUI(currentHealth, currentArmor);
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
            FightEntityStatus statusSelect = currentStatus.Find(status => status.Status == statusType);
            if (statusSelect != null)
            {
                entityUI.AddStatusUI(statusSelect);
                statusSelect.Turn += value;
            }
            else
            {
                FightEntityStatus newStatus = new FightEntityStatus(statusType, value);
                entityUI.AddStatusUI(newStatus);
                currentStatus.Add(newStatus);
            }
        }
        public void RemoveStatus(StatusType statusType)
        {
            FightEntityStatus statusSelect = currentStatus.Find(status => status.Status == statusType);
            if (statusSelect != null)
            {
                entityUI.RemoveStatusUI(statusSelect.Status);
                currentStatus.Remove(statusSelect);
            }
        }
        public bool HasStatus(StatusType statusType)
        {
            return currentStatus.Find(status => status.Status == statusType) != null;
        }
        public int GetValueOfStatus(StatusType statusType)
        {
            FightEntityStatus selectStatus = currentStatus.Find(status => status.Status == statusType);
            return selectStatus == null ? -1 : selectStatus.Turn;
        }
        private void UpdateStatus(FightEntityStatus unitStatus)
        {
            switch (unitStatus.Status)
            {
                case StatusType.Poison:
                    LoseHealth(unitStatus.Turn);
                    break;
            }

            unitStatus.Turn -= 1;
        }
        public void ApplyPoison()
        {
            UpdateStatus(GetStatusByType(StatusType.Poison));
        }
        private FightEntityStatus GetStatusByType(StatusType type)
        {
            return currentStatus.Where(entityStatus => entityStatus.Status == type).FirstOrDefault();
        }
    }
}