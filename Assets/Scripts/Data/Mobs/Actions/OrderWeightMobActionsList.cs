namespace OMG.Data.Mobs.Actions
{
	using System.Collections.Generic;
	using System.Linq;

    using UnityEngine;

    using MVProduction.CustomAttributes;

    [System.Serializable]
	public class OrderWeightMobActionsList : MobActionsList
	{
		[Title("Weight Mob Actions List")]
		[SerializeField] private List<MobActionWeight> mobActionsWeightList;
		
		public List<MobActionWeight> MobActionsWeightList { get => mobActionsWeightList; set => mobActionsWeightList = value; }
		public List<MobActionWeight> OrderMobActionsWeightList { get => mobActionsWeightList; set => mobActionsWeightList = value; }
		
		public override MobActionData GetMobAction()
		{
            mobActionsWeightList.Sort((first, second) => first.Weight.CompareTo(second.Weight));
			int sum = mobActionsWeightList.Sum(x => x.Weight);
			int roll = Random.Range(0,sum);
			
			foreach (MobActionWeight mobActionWeight in mobActionsWeightList)
			{
				sum -= mobActionWeight.Weight;
				
				if (sum <= 0)
				{
					return mobActionWeight.MobAction;
				}
			}
			
			return mobActionsWeightList.Last().MobAction;
		}
	}
}