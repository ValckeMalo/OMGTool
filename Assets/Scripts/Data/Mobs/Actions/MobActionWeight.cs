namespace OMG.Data.Mobs.Actions
{
	using UnityEngine;
	using MVProduction.CustomAttributes;

    [System.Serializable]
    public class MobActionWeight
	{
		[Title("Mob Action Weight")]
		[SerializeField] private MobActionData mobAction = null;
		[SerializeField] private int weight = 0;
		
		public MobActionData MobAction { get => mobAction; set => mobAction = value; }
		public int Weight { get => weight; set => weight = value; }
	}
}