namespace OMG.Data.Mobs.Actions
{
    using System.Collections.Generic;
    using UnityEngine;

    using MVProduction.CustomAttributes;

    [System.Serializable]
    public class SimpleMobActionsList : MobActionsList
    {
        [SerializeField] private List<MobActionData> mobActionsList;

        public List<MobActionData> MobActionsList { get => mobActionsList; set => mobActionsList = value; }

        public override MobActionData GetMobAction()
        {
            int randomIndex = Random.Range(0, mobActionsList.Count);

            return mobActionsList[randomIndex];
        }
    }
}