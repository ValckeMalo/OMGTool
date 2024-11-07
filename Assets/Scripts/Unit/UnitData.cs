namespace OMG.Unit
{
    using MaloProduction.CustomAttributes;
    using System.Collections.Generic;
    using UnityEngine;
    using OMG.Unit.Status;

    [CreateAssetMenu(fileName = "NewUnitData", menuName = "Unit/UnitData", order = 0)]
    public class UnitData : ScriptableObject
    {
        [Title("Data")]
        public int hp;
        public int maxHp;
        public int armor;
        public List<UnitStatus> status = new List<UnitStatus>();
    }
}