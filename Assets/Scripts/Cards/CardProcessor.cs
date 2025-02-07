namespace OMG.Card
{
    using OMG.Battle;
    using OMG.Battle.Data;

    using OMG.Unit;
    using OMG.Unit.Oropo;
    using OMG.Unit.Status;
    using OMG.Unit.Monster;

    using OMG.Card.Data;

    using UnityEngine;

    public static class CardProcessor
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
            ProcessCardType(card.type, valueCard, targets);
            ProcessCardSpells(card.spells, playedFirst, targets);
        }
        public static void ProcessOnlyCardSpells(CardData card, bool playedFirst)
        {
            if (!UnitTest(card)) return; //Failed Unit Test
            if (card.spells == null || card.spells.Length <= 0 || card.type == CardAction.Boost) return;

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

        private static void ProcessCardType(CardAction type, int value, IUnit[] targets)
        {
            switch (type)
            {
                case CardAction.Attack:
                    foreach (IUnit unit in targets)
                    {
                        unit.Damage(value);
                    }
                    break;

                case CardAction.Defense:
                    foreach (IUnit unit in targets)
                    {
                        unit.AddArmor(value);
                    }
                    break;

                case CardAction.Boost:
                    BattleSystem.Instance.GameBoard.BoostAllCard(value);//TODO EVENT ??
                    break;

                case CardAction.Neutral:
                    BattleSystem.Instance.GameBoard.SpawnCardsInHands(value);//TODO EVENT ??
                    break;

                default:
                    Debug.LogError("Youre not supposed to be here, how have you done.");
                    break;
            }
        }
        private static void ProcessCardSpells(Spell[] spells, bool playedFirst, IUnit[] targets)
        {
            foreach (Spell spell in spells)
            {
                if (spell.initiative && !playedFirst) continue;

                ProcessACardSpell(spell, targets);
            }
        }
        private static void ProcessACardSpell(Spell spell, IUnit[] targets)
        {
            switch (spell.type)
            {
                case SpellType.Poison:
                    foreach (IUnit unit in targets)
                    {
                        unit.AddStatus(StatusType.Poison, spell.value);
                    }
                    return;

                case SpellType.Plaie:
                    Oropo.AddStatus(StatusType.Plaie, spell.value);
                    return;

                case SpellType.Shield:
                    Oropo.AddArmor(spell.value);
                    return;

                case SpellType.Tenacite:
                    Oropo.AddStatus(StatusType.Tenacite, spell.value);
                    return;

                case SpellType.Bousculade:
                    Debug.LogError("Bousculade pas implémenter");
                    return;

                case SpellType.Eveil:
                    Oropo.AddStatus(StatusType.Eveil, spell.value);
                    break;

                default:
                    Debug.LogError($"Spell is not recognize or not implemented {spell.type}.");
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

            if (BattleData == null) BattleData = BattleSystem.Instance.BattleData;//TODO ??

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