namespace OMG.Game.Fight.Cards
{
    using OMG.Data.Card;
    using OMG.Game.Fight.Entities;
    using System.Collections.Generic;
    using UnityEngine;

    public static class FightCardProcess
    {
        public static void ProcessCard(FightCard cardPlayed, bool isPlayedFirst, FightData fightData, FightDeck fightDeck)
        {
            if (cardPlayed == null || cardPlayed.Data == null || fightData == null || fightDeck == null)
                return;

            CardData data = cardPlayed.Data;
            FightEntity[] targets = GetTargets(data.target, fightData);
            ProcessCardAction(cardPlayed, targets, fightDeck);

            if (data.spells != null && data.spells.Length > 0)
            {
                ProcessSpells(data.spells, isPlayedFirst, targets, fightDeck);
            }
        }

        public static void ProcessSpells(CardSpell[] spells, bool isPlayedFirst, FightEntity[] targetEntities, FightDeck fightDeck)
        {
            foreach (CardSpell spell in spells)
            {
                if (spell == null)
                    continue;

                ProcessSpell(spell, isPlayedFirst, targetEntities, fightDeck);
            }
        }
        public static void ProcessOnlySpells(FightCard fightCard, bool isPlayedFirst, FightData fightData, FightDeck fightDeck)
        {
            foreach (CardSpell spell in fightCard.Data.spells)
            {
                if (spell == null)
                    continue;

                ProcessSpell(spell, isPlayedFirst, GetTargets(fightCard.Data.target, fightData), fightDeck);
            }
        }
        private static void ProcessSpell(CardSpell spell, bool isPlayedFirst, FightEntity[] targetEntities, FightDeck fightDeck)
        {
            //TODO possiblement un redo des spells plus dans le style des behaviours dans les nouveaux mobs

            if (spell.initiative && !isPlayedFirst)
                return;

            if (spell.type == SpellType.Shield)
            {
                foreach (FightEntity targetEntity in targetEntities)
                {
                    if (targetEntity == null) continue;
                    targetEntity.AddArmor(spell.value);
                }
                return;
            }

            if (spell.type == SpellType.Plaie)
            {
                FightCard plaieCard = new FightCard(spell.cardToSpawn);
                List<FightCard> fightCards = new List<FightCard>();
                for (int i = 0; i < spell.value; i++)
                {
                    fightCards.Add(plaieCard);
                }
                fightDeck.OnDrawMultiplesCard?.Invoke(fightCards);
                return;
            }

            foreach (FightEntity targetEntity in targetEntities)
            {
                if (targetEntity == null) continue;
                targetEntity.AddStatus((StatusType)spell.type, spell.value);
            }
        }
        private static void ProcessCardAction(FightCard fightCard, FightEntity[] targetEntities, FightDeck fightDeck)
        {
            switch (fightCard.CardAction)
            {
                case CardAction.Attack:
                    foreach (FightEntity fightEntity in targetEntities)
                    {
                        if (fightEntity == null) continue;
                        fightEntity.LoseHealth(fightCard.Value);
                    }
                    break;

                case CardAction.Defense:
                    foreach (FightEntity fightEntity in targetEntities)
                    {
                        if (fightEntity == null) continue;
                        fightEntity.AddArmor(fightCard.Value);
                    }
                    break;

                case CardAction.Boost:
                    fightDeck.BoostAllFightCard(fightCard.Value);
                    break;

                case CardAction.Neutral:
                    fightDeck.DrawCards(fightCard.Value);
                    break;

                default:
                    Debug.LogError("Youre not supposed to be here, how have you done.");
                    break;
            }
        }
        private static FightEntity[] GetTargets(Target target, FightData fightData)
        {
            switch (target)
            {
                case Target.FirstMonster:
                    return new FightEntity[1] { fightData.FirstFightMobEntity };

                case Target.LastMonster:
                    return new FightEntity[1] { fightData.LastFightMobEntity };

                case Target.AllMonsters:
                    return fightData.AllMobs.ToArray();

                case Target.Oropo:
                    return new FightEntity[1] { fightData.FightCharacterEntity };

                default:
                    Debug.LogError("PB");
                    return null;
            }
        }
    }
}