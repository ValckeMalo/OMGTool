using UnityEngine;

namespace OMG.Battle.UI.Manager
{
    using UnityEngine;

    using OMG.Unit;
    using OMG.Unit.HUD;
    using MVProduction.CustomAttributes;

    public class UnitHUDSpawner : MonoBehaviour
    {
        public delegate void EventSpawnUnitHUD(Vector3 unitPosition, Unit unit, bool isMonster);
        public static EventSpawnUnitHUD OnSpawnUnitHUD;

        [Header("Settings")]
        [SerializeField] private GameObject prefabUnitHUD;
        [SerializeField, ReadOnly] private Vector2 canvasSizeDelta = Vector2.zero;

        public void Awake() => OnSpawnUnitHUD += SpawnUnitHUD;

        public void SpawnUnitHUD(Vector3 unitPosition, Unit unit, bool isMonster)
        {
            if (canvasSizeDelta == Vector2.zero) canvasSizeDelta = GetComponent<RectTransform>().rect.size;

            GameObject unitHUD = Instantiate(prefabUnitHUD, transform);
            unitHUD.GetComponent<RectTransform>().anchoredPosition = WorldScreen.WorldObjectToScreenPosition(Camera.main, unitPosition, canvasSizeDelta);

            UnitHUD unitHUDComponent = unitHUD.GetComponent<UnitHUD>();

            unitHUDComponent.Initialize(unit, 200f, isMonster);
            unit.SetUnitHUD(unitHUDComponent);
        }
    }
}

public static class WorldScreen
{
    public static Vector2 WorldObjectToScreenPosition(Camera cam, Vector3 objectWorldPosition, Vector2 sizeDeltaCanvas)
    {
        Vector3 viewportPosition = cam.WorldToViewportPoint(objectWorldPosition);
        return new Vector2((viewportPosition.x * sizeDeltaCanvas.x) - (sizeDeltaCanvas.x * 0.5f),
                           (viewportPosition.y * sizeDeltaCanvas.y) - (sizeDeltaCanvas.y * 0.5f));
    }

    public static Vector2 UIObjectToCanvasPosition(Canvas canvas, RectTransform uiObject)
    {
        Vector3 worldPos = uiObject.position;
        Vector2 canvasPosition = Vector2.zero;

        RectTransformUtility.ScreenPointToLocalPointInRectangle
        (
            canvas.GetComponent<RectTransform>(),
            RectTransformUtility.WorldToScreenPoint(null, worldPos),
            canvas.worldCamera,
            out canvasPosition
        );

        return canvasPosition;
    }
}