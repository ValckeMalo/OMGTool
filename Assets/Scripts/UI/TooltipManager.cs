namespace OMG.Battle.UI.Tooltip
{
    using MaloProduction.CustomAttributes;

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
            private Texture icon = null;

            public Type TypeF => type;
            public string Header => header;
            public string Body => body;
            public Texture Icon => icon;

            public TooltipData(Type type, string header, string body, Texture icon)
            {
                this.type = type;
                this.header = header;
                this.body = body;
                this.icon = icon;
            }
        }

        [Title("Tooltip Manager")]
        [SerializeField] private List<Tooltip> tooltipPool = new List<Tooltip>();

        [Header("Tooltip Spawn")]
        [SerializeField] private RectTransform parentTooltip = null;
        [SerializeField] private GameObject tooltipPrefab = null;

        [Header("Tooltip Settings")]
        [SerializeField] private float topMargin = 5f;

        /// <summary>
        /// Displays tooltips with unit data at a specified position and applies a fade-in effect.
        /// </summary>
        /// <param name="startPos">The starting position where the first tooltip will appear.</param>
        /// <param name="fadeDuration">The duration of the fade-in effect for the tooltips.</param>
        /// <param name="tooltipDatas">An array of data objects used to populate the tooltips.</param>
        public void ShowUnitData(Vector2 startPos, float fadeDuration,params TooltipData[] tooltipDatas)
        {//TODO Rename Function
            //Handle some crash
            if (tooltipDatas == null || tooltipDatas.Length <= 0)
            {
                Debug.LogError($"No info pass throught the Tooltip show unit Data");
                return;
            }

            //spawn additional tooltip if the pool is to small compare to data send
            if (tooltipDatas.Length > tooltipPool.Count)
                AddTooltipToPool(tooltipDatas.Length - tooltipPool.Count);

            int indexTooltip = 0;
            Vector2 tooltipPos = startPos;
            float tooltipHeight = -1;
            Tooltip currentTooltip = null;

            foreach (TooltipData tooltipData in tooltipDatas)
            {
                currentTooltip = tooltipPool[indexTooltip];

                currentTooltip.UpdateToNewData(tooltipData);// init the tooltip with the data to show
                currentTooltip.FadeTooltip(1f, fadeDuration);// show the tooltip with a fade made by tween

                tooltipHeight = currentTooltip.GoTo(tooltipPos);// send the tooltip to the destination
                tooltipPos.y -= (tooltipHeight * 2/*WHY NOT*/) + topMargin;// add the height and the margin set, to put the next under it

                indexTooltip++;
            }
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