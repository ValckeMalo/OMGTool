namespace OMG.Battle.UI.Tooltip
{
    using MVProduction.CustomAttributes;

    using System.Collections.Generic;
    using UnityEngine;

    public class TooltipManager : MonoBehaviour
    {
        #region Singleton
        private static TooltipManager instance;
        public static TooltipManager Instance { get { return instance; } }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                return;
            }

            Destroy(gameObject);
        }
        #endregion

        public class TooltipData
        {
            public enum Type
            {
                ACTION,
                STATE,
                CARD,
            }

            private Type type = Type.CARD;
            private string header = string.Empty;
            private string body = string.Empty;
            private Sprite icon = null;

            public Type TypeF => type;
            public string Header => header;
            public string Body => body;
            public Sprite Icon => icon;

            public TooltipData(Type type, string header, string body, Sprite icon)
            {
                this.type = type;
                this.header = header;
                this.body = body;
                this.icon = icon;
            }
        }

        public enum Direction
        {
            Right,
            Left,
        }

        [Title("Tooltip Manager")]
        [SerializeField] private List<Tooltip> tooltipPool = new List<Tooltip>();

        [Header("Tooltip Spawn")]
        [SerializeField] private RectTransform parentTooltip = null;
        [SerializeField] private GameObject tooltipPrefab = null;
        private const float tooltipWidth = 250f;
        private const float tooltipMarginHorizontal = 20f;

        [Header("Tooltip Settings")]
        [SerializeField] private float HeightMargin = 5f;

        ///// <summary>
        ///// Displays tooltips for unit data and card at a specified position and applies a fade-in effect.
        ///// </summary>
        ///// <param name="startPos">The starting position where the first tooltip will appear.</param>
        ///// <param name="fadeDuration">The duration of the fade-in effect for the tooltips.</param>
        ///// <param name="tooltipDatas">An array of data objects used to populate the tooltips.</param>
        //public void ShowUnitData(Vector2 startPos, float fadeDuration, Direction dir, params TooltipData[] tooltipDatas)
        //{//TODO Rename Function
        //    //Handle some crash
        //    if (tooltipDatas == null || tooltipDatas.Length <= 0)
        //    {
        //        Debug.LogError($"No info pass throught the Tooltip show unit Data");
        //        return;
        //    }

        //    //spawn additional tooltip if the pool is to small compare to data send
        //    if (tooltipDatas.Length > tooltipPool.Count)
        //        AddTooltipToPool(tooltipDatas.Length - tooltipPool.Count);

        //    int indexTooltip = 0;
        //    Vector2 tooltipPos = startPos + (new Vector2(tooltipWidth + tooltipMarginHorizontal, 0f) * ((dir == Direction.Right) ? 1 : -1));
        //    float tooltipHeight = -1;
        //    Tooltip currentTooltip = null;

        //    foreach (TooltipData tooltipData in tooltipDatas)
        //    {
        //        currentTooltip = tooltipPool[indexTooltip];

        //        currentTooltip.UpdateToNewData(tooltipData);// init the tooltip with the data to show
        //        currentTooltip.FadeTooltip(1f, fadeDuration);// show the tooltip with a fade made by tween

        //        tooltipHeight = currentTooltip.GoTo(tooltipPos);// send the tooltip to the destination
        //        tooltipPos.y -= (tooltipHeight * 2/*WHY NOT*/) + HeightMargin;// add the height and the margin set, to put the next under it

        //        indexTooltip++;
        //    }
        //}

        /// <summary>
        /// Displays tooltips for unit data at a specified position and applies a fade-in effect.
        /// </summary>
        /// <param name="startPos">The starting position where the first tooltip will appear.</param>
        /// <param name="fadeDuration">The duration of the fade-in effect for the tooltips.</param>
        /// <param name="direction">The direction in which the tooltips will appear (Left or Right).</param>
        /// <param name="tooltipDatas">An array of data objects used to populate the tooltips.</param>
        public void ShowUnitData(Vector2 startPos, float fadeDuration, Direction direction, params TooltipData[] tooltipDatas)
        {
            // Validate the input
            if (tooltipDatas == null || tooltipDatas.Length == 0)
            {
                Debug.LogError("TooltipData array is null or empty. Cannot display tooltips.");
                return;
            }

            EnsureTooltipPoolSize(tooltipDatas.Length);

            Vector2 tooltipPos = CalculateInitialTooltipPosition(startPos, direction);
            for (int i = 0; i < tooltipDatas.Length; i++)
            {
                Tooltip currentTooltip = tooltipPool[i];
                SetupTooltip(currentTooltip, tooltipDatas[i], tooltipPos, fadeDuration);

                tooltipPos = CalculateNextTooltipPosition(tooltipPos, currentTooltip, direction);
            }
        }

        /// <summary>
        /// Ensures the tooltip pool has enough tooltips for the data to be displayed.
        /// </summary>
        private void EnsureTooltipPoolSize(int requiredSize)
        {
            if (requiredSize > tooltipPool.Count)
            {
                AddTooltipToPool(requiredSize - tooltipPool.Count);
            }
        }

        /// <summary>
        /// Calculates the initial position for the first tooltip.
        /// </summary>
        private Vector2 CalculateInitialTooltipPosition(Vector2 startPos, Direction direction)
        {
            float offsetX = (tooltipWidth + tooltipMarginHorizontal) * (direction == Direction.Right ? 1 : -1);
            return startPos + new Vector2(offsetX, 0f);
        }

        /// <summary>
        /// Sets up the tooltip with data, fades it in, and positions it.
        /// </summary>
        private void SetupTooltip(Tooltip tooltip, TooltipData data, Vector2 position, float fadeDuration)
        {
            tooltip.UpdateToNewData(data);
            tooltip.FadeTooltip(1f, fadeDuration);
            tooltip.GoTo(position);
        }

        /// <summary>
        /// Calculates the position for the next tooltip based on the current tooltip's height and margin.
        /// </summary>
        private Vector2 CalculateNextTooltipPosition(Vector2 currentPosition, Tooltip currentTooltip, Direction direction)
        {
            float tooltipHeight = currentTooltip.GetHeight(); // Assuming there's a method to get the tooltip height
            return new Vector2(currentPosition.x, currentPosition.y - (tooltipHeight + HeightMargin));
        }


        /// <summary>
        /// Hides all active tooltips by applying a fade-out effect.
        /// </summary>
        /// <param name="fadeDuration">The duration of the fade-out effect.</param>
        public void HideUnitData(float fadeDuration)
        {
            foreach (Tooltip tooltip in tooltipPool)
            {
                tooltip.FadeTooltip(0f, fadeDuration);
            }
        }

        /// <summary>
        /// Adds additional tooltips to the pool to match the required number of tooltips.
        /// </summary>
        /// <param name="nbToSpawn">The number of tooltips to spawn and add to the pool.</param>
        private void AddTooltipToPool(int nbToSpawn)
        {
            if (parentTooltip == null || tooltipPrefab == null)
            {
                Debug.LogError($"None parent detected to spawn tooltip {parentTooltip} or the prefab is null {tooltipPrefab}");
                return;
            }

            GameObject newTooltipObject = null;
            Tooltip tooltipComponent = null;

            for (int i = 0; i < nbToSpawn; i++)
            {
                newTooltipObject = Instantiate(tooltipPrefab, parentTooltip);
                tooltipComponent = newTooltipObject.GetComponent<Tooltip>();

                if (tooltipComponent == null)
                {
                    Debug.LogError("None component found for tooltip");
                    continue;
                }

                tooltipComponent.Init();
            }
        }
    }
}