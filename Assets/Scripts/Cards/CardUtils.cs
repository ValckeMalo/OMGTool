namespace OMG.Card
{
	using OMG.Battle;
	using OMG.Unit;
	using OMG.Unit.Status;
	using static CardData;

	using System.Collections.Generic;
	using UnityEngine;

	public static class CardUtils
	{
		public static void ProcessCard(CardData card, bool playedFirst)
		{
			IUnit[] unitsTarget = GetUnitsTarget(card.target);
			ProcessCardType(card.cardType, card.cardValue, unitsTarget);
			ProcessCardSpells(BattleSystem.BattleData.GetOropo(), BattleSystem.BattleData.GetAllMonsters(), unitsTarget, card.spells, playedFirst);
		}

		private static void ProcessCardType(CardType type, int cardValue, IUnit[] units)
		{
			switch (type)
			{
				case CardType.Attack:
					foreach (IUnit unit in units)
					{
						if (unit != null)
						{
							unit.Damage(cardValue);
						}
					}
					return;

				case CardType.Defense:
					foreach (IUnit unit in units)
					{
						if (unit != null)
						{
							unit.AddArmor(cardValue);
						}
					}
					return;

				case CardType.Boost:
					Debug.LogWarning("Boost");
					return;

				case CardType.Neutral:
					Debug.LogWarning("Neutral");
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

		private static void ProcessCardSpells(IUnit oropo, IUnit[] monsters, IUnit[] unitsTarget, List<Spell> spells, bool playedFirst)
		{
			foreach (Spell spell in spells)
			{
				ProcessSpell(oropo, monsters, unitsTarget, spell, playedFirst);
			}
		}

		private static void ProcessSpell(IUnit oropo, IUnit[] monsters, IUnit[] unitsTarget, Spell spell, bool playedFirst)
		{
			bool CanDoSpell()
			{
				return (playedFirst && spell.isInitiative) || !spell.isInitiative;
			}

			switch (spell.spellType)
			{
				case Spells.Poison:
					if (CanDoSpell())
					{
						foreach (IUnit unit in unitsTarget)
						{
							if (unit != null)
							{
								unit.AddStatus(StatusType.Poison, spell.amount);
							}
						}
					}
					return;

				case Spells.Plaie:
					if (CanDoSpell())
					{
						if (oropo != null)
						{
							oropo.AddStatus(StatusType.Plaie, spell.amount);
						}
					}
					return;

				case Spells.Shield:
					if (CanDoSpell())
					{
						if (oropo != null)
						{
							oropo.AddArmor(spell.amount);
						}
					}
					return;

				case Spells.Tenacite:
					if (CanDoSpell())
					{
						if (oropo != null)
						{
							oropo.AddStatus(StatusType.Tenacite, spell.amount);
						}
					}
					return;

				case Spells.Sacrifice:
					Debug.LogError("Sacrifice n'est pas un spell");
					return;

				case Spells.Bousculade:
					Debug.LogError("Bousculade pas implémenter");
					return;

				case Spells.Eveil:
					if (CanDoSpell())
					{
						if (oropo != null)
						{
							oropo.AddStatus(StatusType.Eveil, spell.amount);
						}
					}
					break;

				default:
					Debug.LogError($"Spell is not recognize or not implemented {spell.spellType}.");
					break;
			}
		}

		private static IUnit[] GetUnitsTarget(Target target)
		{
			if (BattleSystem.BattleData == null)
			{
				Debug.LogError($"The BattleData is equal to null");
				return null;
			}

			List<IUnit> units = new List<IUnit>();

			switch (target)
			{
				case Target.FirstMonster:
					units.Add(BattleSystem.BattleData.GetFirstMonster());
					break;

				case Target.LastMonster:
					units.Add(BattleSystem.BattleData.GetLastMonster());
					break;

				case Target.AllMonsters:
					return BattleSystem.BattleData.GetAllMonsters();

				case Target.Oropo:
					units.Add(BattleSystem.BattleData.GetOropo());
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
	}
}