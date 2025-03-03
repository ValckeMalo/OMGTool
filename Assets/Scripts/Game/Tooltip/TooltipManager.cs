namespace OMG.Game.Tooltip
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;

    public class TooltipManager : MonoBehaviour
    {
        public delegate List<GameObject> EventSpawnTooltips(List<TooltipData> tooltipDatas);
        public static EventSpawnTooltips OnSpawnTooltip;
        public static Action<List<GameObject>> OnReleaseTooltipObject;

        [Header("Pool")]
        [SerializeField] private Transform containerPoolTooltip;
        [SerializeField] private GameObject tooltipPrefab;
        private List<GameObject> poolTooltips = new List<GameObject>();

        private void Awake()
        {
            OnSpawnTooltip += SpawnTooltip;
            OnReleaseTooltipObject += ReleaseTooltipObject;

            if (containerPoolTooltip != null)
            {
                for (int i = 0; i < containerPoolTooltip.childCount; i++)
                {
                    if (containerPoolTooltip.GetChild(i) != null && 
                        containerPoolTooltip.GetChild(i).GetComponent<TooltipUI>() != null)
                    {
                        poolTooltips.Add(containerPoolTooltip.GetChild(i).gameObject);
                    }
                }
            }
        }

        private List<GameObject> SpawnTooltip(List<TooltipData> tooltipDatas)
        {
            if (tooltipDatas == null || tooltipDatas.Count <= 0)
                return null;

            List<GameObject> tooltipsobject = new List<GameObject>();

            foreach (TooltipData tooltipData in tooltipDatas)
            {
                if (tooltipData == null) continue;

                GameObject obj = null;
                if (poolTooltips.Count > 0)
                {
                    //get the tooltip from the pool
                    obj = poolTooltips[0];
                    poolTooltips.Remove(obj);
                }
                else
                {
                    //spawn a new tooltip because the pool is empty
                    obj = Instantiate(tooltipPrefab, containerPoolTooltip);
                }

                //add the obj to the return value
                tooltipsobject.Add(obj);

                //change or set is ui base on the data send
                TooltipUI tooltipUI = obj.GetComponent<TooltipUI>();
                tooltipUI.SetTooltipData(tooltipData);
                tooltipUI.Show();
            }

            return tooltipsobject;
        }

        private void ReleaseTooltipObject(List<GameObject> tooltipsObject)
        {
            if (tooltipsObject == null || tooltipsObject.Count <= 0)
                return;

            foreach (GameObject obj in  tooltipsObject)
            {
                if (obj == null)
                    continue;

                obj.GetComponent<TooltipUI>().Hide();
                obj.transform.SetParent(containerPoolTooltip, false);
                poolTooltips.Add(obj);
            }
        }
    }
}