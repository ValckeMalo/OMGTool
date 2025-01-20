namespace OMG.Unit.HUD
{
    using MVProduction.CustomAttributes;

    using OMG.Unit.Status;

    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class StatusUI : MonoBehaviour
    {
        public StatusType Status { get; private set; }

        [Title("Status UI")]
        [SerializeField] private TextMeshProUGUI turnText;
        [SerializeField] private Image iconStatus;

        public void Initialize(int numberStatus, StatusType status)
        {
            UpdateTextTurn(numberStatus);
            Status = status;
        }

        public void UpdateTextTurn(int turn)
        {
            turnText.text = turn.ToString();
        }

        public void Destroy() => Destroy(gameObject);
    }
}