namespace OMG.Battle.Manager
{
    using OMG.Battle.UI;

    using UnityEngine;

    public class WakfuManager
    {
        private int wakfu;
        private int wakfuUnlock;
        private bool hasToRemovePadlock;

        private const int MaxWakfu = 6;

        public int WakfuRemain => wakfuUnlock - wakfu;

        public WakfuManager()
        {
            ResetWakfu();
        }
        private void ResetWakfu()
        {
            hasToRemovePadlock = false;
            wakfu = 0;
            wakfuUnlock = 3;
            HUDBattle.OropoWakfuGauge.ResetPadLock();
        }

        public void PreviewWakfu(int amount)
        {
            int preivewWakfu = Mathf.Clamp(wakfu + amount, 0, MaxWakfu);
            HUDBattle.OropoWakfuGauge.UpdatePreviewBar(wakfu + preivewWakfu);
        }

        public bool CanAddWakfu(int amount)
        {
            return (wakfu + amount <= wakfuUnlock);
        }

        public void AddWakfu(int wakfuToAdd)
        {
            wakfu += wakfuToAdd;
            wakfu = Mathf.Max(wakfu, 0);
            HUDBattle.OropoWakfuGauge.UpdateWakfuBar(wakfu);
        }

        public void UnlockWakfu()
        {
            if (wakfu == MaxWakfu)
            {
                ResetWakfu();
            }
            else
            {
                wakfu = 0;
                wakfuUnlock++;

                if (wakfuUnlock <= MaxWakfu)//if the wakfu unlock havn't exceed max possible break a pad lock on UI
                {
                    HUDBattle.OropoWakfuGauge.BreakPadLock();
                    hasToRemovePadlock = true;
                }
            }

            //reset UI
            HUDBattle.OropoWakfuGauge.ResetGauges();
        }

        public void TryBreakPadLock()
        {
            if (hasToRemovePadlock)
            {
                HUDBattle.OropoWakfuGauge.RemovePadLock();
                hasToRemovePadlock = false;
            }
        }
    }
}