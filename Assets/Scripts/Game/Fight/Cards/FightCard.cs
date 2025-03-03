namespace OMG.Game.Fight.Cards
{
    using OMG.Data.Card;
    using System;

    public class FightCard
    {
        private readonly CardData card = null;
        private int energyBonus = 0;
        private int valueBonus = 0;

        public CardData Data => card;
        public CardBackground CardType => card.background;
        public CardAction CardAction => card.type;
        public int Energy => card.energy + energyBonus;
        public int Value => card.value + valueBonus;

        public bool HadValueBonus => valueBonus != 0;
        public bool HadEnergyBonus => energyBonus != 0;
        public bool NeedAnotherCard => card.needSacrifice || card.target == Target.OneCard;
        public bool NeedSacrifice => card.needSacrifice;
        public bool IsBoostable => card.isBoostable;
        private bool HoldDamage => false;//TODO MAKE IT if the card keep the modify value beacause his spell did it		

        public Action OnCardUpdated;
        public Action OnCardDestroyed;

        public FightCard(CardData cardData)
        {
            card = cardData;
            OnCardDestroyed += ResetCard;
        }

        private void ResetCard()
        {
            //reset all additional values stored on the card when it has been used or destroyed
            energyBonus = 0;

            //if the card doesn't have the spell to keep the additional values
            if (!HoldDamage)
            {
                valueBonus = 0;
            }
        }

        #region Boost
        public void BoostValue(int boostValue)
        {
            valueBonus += boostValue;
            OnCardUpdated?.Invoke();
        }
        #endregion
    }
}