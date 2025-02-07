namespace OMG.Data.Mobs.Actions
{
    using System.Collections.Generic;
    using UnityEngine;

    using MVProduction.CustomAttributes;

	public class SimpleMobActionsList : MobActionsList
	{
		[Title("Simple Mob Actions List")]
		[SerializeField] private List<MobActionTarget> mobActionsList;
		
		public List<MobActionTarget> MobActionsList { get => mobActionsList; }
		
		public override MobActionTarget GetMobAction()
		{
			int randomIndex = Random.Range(0,mobActionsList.Count);
			
			return mobActionsList[randomIndex];
		}
	}
}