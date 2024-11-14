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
        public bool HaveStatus(StatusType statusType)
        {
            return status.Where(x => x.status == statusType).FirstOrDefault() != null;
        }
    }
}