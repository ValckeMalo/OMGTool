namespace OMG.Unit
{
    using MaloProduction.CustomAttributes;

    using System.Collections.Generic;
    using System.Linq;

    using OMG.Unit.Status;

    [System.Serializable]
    public class UnitData
    {
        [Title("Data")]
        public int hp;
        public int maxHp;
        public int armor;
        public List<UnitStatus> status = new List<UnitStatus>();
        private List<UnitStatus> toKillStatus = null;
        public string unitName;

        public bool HaveStatus(StatusType statusType)
        {
            return status.Where(x => x != null && x.status == statusType).FirstOrDefault() != null;
        }
        public int GetStatusValue(StatusType statusType)
        {
            return status.Where(x => x != null && x.status == statusType).FirstOrDefault().turn;
        }
        public void UpdateAllStatus()
        {
            if (status == null) UnityEngine.Debug.LogError($"The collections of unit Status in {unitName} are equal to null : {status}.");

            foreach (UnitStatus unitStatus in status)
            {
                if (unitStatus == null) continue;
                unitStatus.turn--;
                if (unitStatus.turn <= 0) MarkForKillStatus(unitStatus);
            }

            KillAllMarkedStatus();
        }

        private void MarkForKillStatus(UnitStatus unitStatus)
        {
            if (toKillStatus == null) toKillStatus = new List<UnitStatus>();

            toKillStatus.Add(unitStatus);
        }
        private void KillAllMarkedStatus()
        {
            if (toKillStatus == null) return; // There is no status to kill

            foreach (UnitStatus statusToKill in toKillStatus)
            {
                status.Remove(statusToKill);
            }

            toKillStatus.Clear();
            toKillStatus = null;
        }
    }
}