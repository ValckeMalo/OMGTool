namespace OMG.Battle.UI.Manager
{
    using OMG.Unit;
    using OMG.Unit.HUD;
    using UnityEngine;

    [System.Serializable]
    public class UnitsHUDManager
    {
        [SerializeField] private RectTransform parent;
        [SerializeField] private RectTransform canvasTransform;
        [SerializeField] private GameObject prefabUnitHUD;

        public void SpawnUnitHUD(Vector3 unitPosition, Unit unit, bool isMonster)
        {
            Vector3 viewportPosition = Camera.main.WorldToViewportPoint(unitPosition);
            Vector2 worldObjectScreenPosition = new Vector2(
                                    ((viewportPosition.x * canvasTransform.sizeDelta.x) - (canvasTransform.sizeDelta.x * 0.5f)),
                                    ((viewportPosition.y * canvasTransform.sizeDelta.y) - (canvasTransform.sizeDelta.y * 0.5f)));

            GameObject unitHUD = UnityEngine.Object.Instantiate(prefabUnitHUD, parent);
            unitHUD.GetComponent<RectTransform>().anchoredPosition = worldObjectScreenPosition;
            unitHUD.GetComponent<UnitHUD>().Initialize(unit, 200f, isMonster);
            unit.SetUnitHUD(unitHUD.GetComponent<UnitHUD>());
        }
    }
}