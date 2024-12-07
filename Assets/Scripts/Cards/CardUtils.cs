namespace OMG.Card
{
    using OMG.Battle;
    using OMG.Battle.Data;
    using OMG.Unit;
    using OMG.Unit.Oropo;
    using OMG.Unit.Status;
    using OMG.Unit.Monster;
    using static CardData;

    using UnityEngine;
    using System.Collections.Generic;

    public static class CardUtils
    {
        private static BattleData BattleData;

        private static Player Oropo => BattleData.GetOropo();
        private static Monster FirstMonster => BattleData.GetFirstMonster();
        private static Monster LastMonster => BattleData.GetLastMonster();
        private static Monster[] Monsters => BattleData.GetAllMonsters();

        #region Process
        public static void ProcessCard(CardData card, int valueCard, bool playedFirst)
        {
            if (!UnitTest(card)) return; //Failed Unit Test

            IUnit[] targets = GetTargets(card.target);
            ProcessCardType(card, targets);
            ProcessCardSpells(card.spells, playedFirst, targets);
        }
        public static void ProcessOnlyCardSpells(CardData card, bool playedFirst)
        {
            if (!UnitTest(card)) return; //Failed Unit Test

            IUnit[] targets = GetTargets(card.target);
            ProcessCardSpells(card.spells, playedFirst, targets);
        }

        private static IUnit[] GetTargets(Target target)
        {
            IUnit[] targets = new IUnit[1] { Oropo };
            switch (target)
            {
                case Target.FirstMonster:
                    return new IUnit[1] { FirstMonster };

                case Target.LastMonster:
                    return new IUnit[1] { LastMonster };

                case Target.AllMonsters:
                    return Monsters;

                case Target.Oropo:
                    return new IUnit[1] { Oropo };

                default:
                    Debug.LogError("PB");
                    return null;
            }
        }

        private static void ProcessCardType(CardData card, IUnit[] targets)
        {
            CardType cardType = card.cardType;
            int cardValue = card.cardValue;

            if (cardType == CardType.Divine || cardType == CardType.Curse || cardType == CardType.Finisher)
            {
                cardType = card.specialCardType;
            }

            switch (cardType)
            {
                case CardType.Attack:
                    foreach (IUnit unit in targets)
                    {
                        unit.Damage(cardValue);
                    }
                    break;

                case CardType.Defense:
                    foreach (IUnit unit in targets)
                    {
                        unit.AddArmor(cardValue);
                    }
                    break;

                case CardType.BoostMultiple:
                    BattleSystem.Instance.GameBoard.BoostAllCard(cardValue);
                    break;

                case CardType.Neutral:
                    BattleSystem.Instance.GameBoard.SpawnCardsInHands(cardValue);
                    break;

                case CardType.BoostSingle:
                case CardType.Finisher:
                case CardType.Divine:
                case CardType.Curse:
                default:
                    Debug.LogError("Youre not supposed to be here, how have you done.");
                    break;
            }
        }
        private static void ProcessCardSpells(List<Spell> spells, bool playedFirst, IUnit[] targets)
        {
            foreach (Spell spell in spells)
            {
                if (!playedFirst && spell.isInitiative) continue;

                ProcessACardSpell(spell, targets);
            }
        }
        private static void ProcessACardSpell(Spell spell, IUnit[] targets)
        {
            switch (spell.spellType)
            {
                case Spells.Poison:
                    foreach (IUnit unit in targets)
                    {
                        unit.AddStatus(StatusType.Poison, spell.amount);
                    }
                    return;

                case Spells.Plaie:
                    Oropo.AddStatus(StatusType.Plaie, spell.amount);
                    return;

                case Spells.Shield:
                    Oropo.AddArmor(spell.amount);
                    return;

                case Spells.Tenacite:
                    Oropo.AddStatus(StatusType.Tenacite, spell.amount);
                    return;

                case Spells.Bousculade:
                    Debug.LogError("Bousculade pas implémenter");
                    return;

                case Spells.Eveil:
                    Oropo.AddStatus(StatusType.Eveil, spell.amount);
                    break;

                default:
                    Debug.LogError($"Spell is not recognize or not implemented {spell.spellType}.");
                    break;
            }
        }
        #endregion

        #region Unit Test
        private static bool UnitTest(CardData card)
        {
            if (card == null)
            {
                Debug.LogError($"The card send is null : {card}");
                return false;
            }

            if (BattleData == null) BattleData = BattleSystem.Instance.BattleData;

            return UnitTestBattleData();
        }
        private static bool UnitTestBattleData()
        {
            if (BattleData == null)
            {
                Debug.LogError($"The {BattleData.name} is equal to null in The Card Utils : {BattleData}");
                return false;
            }

            if (Oropo == null)
            {
                Debug.LogError($"The {Oropo.name} is equal to null in The Card Utils : {Oropo}");
                return false;
            }

            if (FirstMonster == null)
            {
                Debug.LogError($"The {FirstMonster.name} is equal to null in The Card Utils : {FirstMonster}");
                return false;
            }

            if (LastMonster == null)
            {
                Debug.LogError($"The {LastMonster.name} is equal to null in The Card Utils : {LastMonster}");
                return false;
            }

            if (Monsters == null)
            {
                Debug.LogError($"The Monsters is equal to null in The Card Utils : {Monsters}");
                return false;
            }

            return true;
        }
        #endregion
    }
}