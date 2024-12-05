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

        private static bool needSecondCard = false;
        private static PlayableCard firstCard = null;

        public static bool ProcessCard(PlayableCard playableCard, bool playedFirst)
        {
            CardData card = playableCard.CardData;

            if (!UnitTest(card)) return false; //Failed Unit Test

            if (needSecondCard)
            {
                ProcessSecondCard(playedFirst); //For boost and sacrifice card
                return true;
            }

            if (IsNeedSecondCard(card))//Test whether the card needs a second click card to work
            {
                needSecondCard = true;
                firstCard = playableCard;
                return false;
            }

            IUnit[] unitsTarget = GetUnitsTarget(card.target); //Get the targets of the cards

            ProcessCardType(card.cardType, card.cardValue, unitsTarget); //Process the card type (do what the base card it seems to do) Attack,Defense,Neutral etc...
            ProcessCardSpells(unitsTarget, card.spells, playedFirst); //Process the spells of the card

            return true;
        }

        private static void ProcessCardType(CardType type, int cardValue, IUnit[] units)
        {
            switch (type)
            {
                case CardType.Attack:
                    foreach (IUnit unit in units)
                    {
                        unit.Damage(cardValue);
                    }
                    return;

                case CardType.Defense:
                    foreach (IUnit unit in units)
                    {
                        unit.AddArmor(cardValue);
                    }
                    return;

                case CardType.BoostMultiple:
                    Debug.LogWarning("Boost");
                    return;

                case CardType.Neutral:
                    BattleSystem.Instance.GameBoard.SpawnCardsInHands(cardValue);
                    return;

                case CardType.Divine:
                    Debug.LogWarning("Divine");
                    return;

                case CardType.Curse:
                    Debug.LogWarning("Curse");
                    return;

                case CardType.Finisher:
                    Debug.LogWarning("Finisher");
                    return;

                default:
                    Debug.LogError($"The card type is not recognize : {type}");
                    return;
            }
        }
        private static void ProcessCardSpells(IUnit[] unitsTarget, List<Spell> spells, bool playedFirst)
        {
            if (unitsTarget == null)
            {
                Debug.LogError($"Can't find Units Target : {unitsTarget}.");
                return;
            }

            foreach (Spell spell in spells)
            {
                ProcessSpell(unitsTarget, spell, playedFirst);
            }
        }
        private static void ProcessSpell(IUnit[] unitsTarget, Spell spell, bool playedFirst)
        {
            if (!playedFirst && spell.isInitiative)
            {
                Debug.Log($"The card have a spell in initiative but the cards was not played first.");
                return;
            }

            switch (spell.spellType)
            {
                case Spells.Poison:
                    foreach (IUnit unit in unitsTarget)
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
        private static IUnit[] GetUnitsTarget(Target target)
        {
            List<IUnit> units = new List<IUnit>();

            switch (target)
            {
                case Target.FirstMonster:
                    units.Add(FirstMonster);
                    break;

                case Target.LastMonster:
                    units.Add(LastMonster);
                    break;

                case Target.AllMonsters:
                    return Monsters;

                case Target.Oropo:
                    units.Add(Oropo);
                    break;

                default:
                    Debug.LogError($"The target in the card is not recognize : {target} : {(int)target}");
                    return null;
            }

            if (units == null || units.Count <= 0)
            {
                Debug.LogError($"Can't find a unit to fit with the target or the unit is equal to null : {target}");
                return null;
            }

            return units.ToArray();
        }

        private static bool ProcessSecondCard(bool playedFirst)
        {
            if (firstCard == null) return false;
            CardData firstCardData = firstCard.CardData;

            if (firstCardData.cardType == CardType.BoostMultiple)
            {

            }
            else if (firstCardData.needSacrifice)
            {
                IUnit[] unitsTarget = GetUnitsTarget(firstCardData.target); //Get the targets of the cards

                ProcessCardType(firstCardData.cardType, firstCardData.cardValue, unitsTarget); //Process the card type (do what the base card it seems to do) Attack,Defense,Neutral etc...
                ProcessCardSpells(unitsTarget, firstCardData.spells, playedFirst); //Process the spells of the card

                firstCard = null;
                needSecondCard = false;

                BattleSystem.Instance.GameBoard.RemoveCardOnBoard(firstCard);

                return true;
            }

            return false;
        }
        private static bool IsNeedSecondCard(CardData card)
        {
            return card.needSacrifice || card.cardType == CardType.BoostMultiple;
        }

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


        #region NEW PORCESS
        public static void ProcessCard2(CardData card, bool playedFirst)
        {
            if (!UnitTest(card)) return; //Failed Unit Test

            IUnit[] targets = GetTargets2(card.target);
            ProcessCardType2(card.cardType, card.cardValue, targets);
            ProcessCardSpells2(card.spells, playedFirst, targets);
        }
        public static void ProcessOnlyCardSpells(CardData card, bool playedFirst)
        {
            if (!UnitTest(card)) return; //Failed Unit Test

            IUnit[] targets = GetTargets2(card.target);
            ProcessCardSpells2(card.spells, playedFirst, targets);
        }


        private static IUnit[] GetTargets2(Target target)
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

        private static void ProcessCardType2(CardType type, int cardValue, IUnit[] targets)
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
                    Debug.LogError("PB");
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
        private static void ProcessCardSpells2(List<Spell> spells, bool playedFirst, IUnit[] targets)
        {
            foreach (Spell spell in spells)
            {
                if (!playedFirst && spell.isInitiative) continue;

                ProcessACardSpell2(spell, targets);
            }
        }
        private static void ProcessACardSpell2(Spell spell, IUnit[] targets)
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
    }
}