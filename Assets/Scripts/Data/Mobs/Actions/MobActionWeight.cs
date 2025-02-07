namespace OMG.Data.Mobs.Actions
{
	using UnityEngine;
	using MVProduction.CustomAttributes;

    [System.Serializable]
    public class MobActionWeight
	{
		[Title("Mob Action Weight")]
		[SerializeField] private MobActionTarget mobAction = null;
		[SerializeField] private int weight = 0;
		
		public MobActionTarget MobAction { get => mobAction; }
		public int Weight { get => weight; }
	}
}