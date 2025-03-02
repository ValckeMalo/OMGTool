namespace OMG.Game.Fight.Entities
{
    using MVProduction.CustomAttributes;

    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class FightEntityStatusUI : MonoBehaviour
    {
        [Title("Status UI")]
        [SerializeField] private TextMeshProUGUI turnText;
        [SerializeField] private Image iconStatus;
        [SerializeField] private EntityStatusUIData statusUIData;
		private StatusType statusType;
		
		public StatusType StatusType => statusType;
		public string Title => statusUIData.GetTitleByKey(statusType);
		public string Description => statusUIData.GetDescriptionByKey(statusType);
		public Sprite Icon => iconStatus.sprite;

        public void Initialize(StatusType statusType, int turnValue)
        {
            UpdateTextTurn(turnValue);
            this.statusType = statusType;
			
			if (statusUIData == null)
				Debug.LogError("The data for the status is not set !!");
			else
				iconStatus.sprite = statusUIData.GetIconByKey(this.statusType);
        }

        public void UpdateTextTurn(int turn)
        {
            turnText.text = turn.ToString();
        }
    }
}