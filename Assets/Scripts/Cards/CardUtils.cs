namespace OMG.Card
{
    using OMG.Battle;
    using OMG.Unit;
    using OMG.Unit.Status;
    using static CardData;
    using OMG.Battle.Data;
    using OMG.Unit.Oropo;
    using OMG.Unit.Monster;

    using System.Collections.Generic;
    using UnityEngine;
    using OMG.Card.UI;
    using System;

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
            ProcessCardType(card.cardType, valueCard, targets);
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

        private static void ProcessCardType(CardType type, int cardValue, IUnit[] targets)
        {
            switch (type)
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

                case CardType.BoostSingle:
                    Debug.LogError("PB");
                    break;

                case CardType.BoostMultiple:
                    BattleSystem.Instance.GameBoard.BoostAllCard(cardValue);
                    break;

                case CardType.Neutral:
                    BattleSystem.Instance.GameBoard.SpawnCardsInHands(cardValue);
                    break;

                case CardType.Divine:
                    Debug.LogError("PB");
                    break;

                case CardType.Curse:
                    Debug.LogError("PB");
                    break;

                case CardType.Finisher:
                    Debug.LogError("PB");
                    break;

                default:
                    Debug.LogError("PB");
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