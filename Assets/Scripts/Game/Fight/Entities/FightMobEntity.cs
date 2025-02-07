namespace OMG.Game.Fight.Entities
{
    using OMG.Data.Mobs.Actions;
    using OMG.Data.Mobs.Behaviour;
    using OMG.Data.Utils;
    using OMG.Unit.Monster;

    using System;
    using UnityEngine;

    [Serializable]
    public class FightMobEntity : FightEntity
    {
        private Monster/*Rename to MobData*/ mobData;
        private MobActionTarget nextMobAction;
        private Action<MobActionTarget> onNextActionUpdate = null;

        public void InitializeMob(int currentHealth, int maxHealth, Monster mobData/*,TODO add UI*/)
        {
            this.mobData = mobData;
            InitializeEntity(currentHealth, maxHealth);
        }

        public override void NewTurn()
        {
            base.NewTurn();

            PlayNextAction();
        }

        public override void EndTurn()
        {
            SearchNextAction();
        }

        private void SearchNextAction()
        {
            if (mobData == null || mobData.MobFightBehaviours == null) return;

            foreach (MobFightBehaviour behaviour in mobData.MobFightBehaviours)
            {
                if (behaviour == null) continue;
                if (behaviour.PrimaryFightCondition == null) continue;

                bool primaryCondition = AvailableBehaviour(behaviour.PrimaryFightCondition);
                if (behaviour.SecondaryFightCondition == null)
                {
                    if (primaryCondition)
                    {
                        SetNextAction(behaviour.MobActionList.GetMobAction());
                    }
                }
                else
                {
                    bool secondaryCondition = AvailableBehaviour(behaviour.SecondaryFightCondition);

                    if (behaviour.ConditionOperator == ConditionOperator.And && primaryCondition && secondaryCondition)
                    {
                        SetNextAction(behaviour.MobActionList.GetMobAction());
                    }
                    else if (behaviour.ConditionOperator == ConditionOperator.Or && (primaryCondition || secondaryCondition))
                    {
                        SetNextAction(behaviour.MobActionList.GetMobAction());
                    }
                }
            }

        }
        private void PlayNextAction()
        {
            if (nextMobAction == null) return;

            FightEntity fightEntityTarget = GetTarget(nextMobAction.Target);

            //nextMobAction.MobAction.Execute(fightEntityTarget);//TODO change the execute line with the fight entity

            nextMobAction = null;
        }
        private FightEntity GetTarget(FightEntityTarget target)
        {
            switch (target)
            {
                case FightEntityTarget.Me:
                    return this;

                case FightEntityTarget.Player:
                    return FightLogic.instance.FightCharacterEntity;

                case FightEntityTarget.FirstMob:
                    return FightLogic.instance.FirstFightMobEntity;

                case FightEntityTarget.LastMob:
                    return FightLogic.instance.LastFightMobEntity;

                case FightEntityTarget.RandomMob:
                    return FightLogic.instance.GetRandomMob();

                case FightEntityTarget.AllMobs:
                    return null; /*FightLogic.instance.FightMobEntities;//TODO MEH Found a way to correct that*/

                case FightEntityTarget.WeakestMob:
                    return FightLogic.instance.WeakestFightMobEntity();

                case FightEntityTarget.RandomMobOther:
                    return FightLogic.instance.GetRandomMob();//TODO set the pos of the current mob to execpt it for the random

                case FightEntityTarget.RandomMobWithState:
                    return FightLogic.instance.GetRandomMobWithStatus(Unit.Status.StatusType.Poison); //TODO add a wrapper on the status type to add specific data like in the condition behaviour

                case FightEntityTarget.RandomAll:
                    return FightLogic.instance.GetRandomEntity();

                default:
                    Debug.LogError($"Default switch reach {target} for target");
                    return null;
            }
        }

        private void SetNextAction(MobActionTarget nextMobAction)
        {
            this.nextMobAction = nextMobAction;
            onNextActionUpdate?.Invoke(nextMobAction);
        }

        private bool ComparisonOperatorValue(ComparisonOperator comparisonOperator, float value, float treshold)
        {
            switch (comparisonOperator)
            {
                case ComparisonOperator.Greater:
                    return value > treshold;

                case ComparisonOperator.GreaterOrEqual:
                    return value >= treshold;

                case ComparisonOperator.Equal:
                    return value == treshold;

                case ComparisonOperator.LessOrEqual:
                    return value <= treshold;

                case ComparisonOperator.Less:
                    return value < treshold;

                default:
                    Debug.LogError($"Default switch reach {comparisonOperator} for condition operator");
                    return false;
            }
        }
        private bool ComparisonMobPosition(FightMobEntity mobAtPos, FightPosition positionNeed)
        {
            switch (positionNeed)
            {
                case FightPosition.First:
                case FightPosition.Last:
                    return this == mobAtPos;

                case FightPosition.NotFirst:
                case FightPosition.NotLast:
                    return this != mobAtPos;

                default:
                    Debug.LogError($"Default switch reach {positionNeed} for condition position");
                    return false;
            }
        }

        private bool AvailableBehaviour(FightCondition fightConditon)
        {
            switch (fightConditon.ConditionType)
            {
                case FightConditionType.None:
                    return true;

                case FightConditionType.MobHealthPercent:
                    return ComparisonOperatorValue(fightConditon.ComparisionOperator, EntityPercentHealth, fightConditon.SpecificValue);

                case FightConditionType.CharacterHealthPercent:
                    return ComparisonOperatorValue(fightConditon.ComparisionOperator, FightLogic.instance.FightCharacterEntity.EntityPercentHealth, fightConditon.SpecificValue);

                case FightConditionType.MobsCount:
                    return ComparisonOperatorValue(fightConditon.ComparisionOperator, FightLogic.instance.MobCount, fightConditon.SpecificValue);

                case FightConditionType.FightTurn:
                    return ComparisonOperatorValue(fightConditon.ComparisionOperator, FightLogic.instance.NbTurn, fightConditon.SpecificValue);

                case FightConditionType.MobOnBoardWithHealth:
                    return ComparisonOperatorValue(fightConditon.ComparisionOperator, EntityPercentHealth, fightConditon.SpecificValue);//TODO change to search one mob

                case FightConditionType.CharacterArmor:
                    return ComparisonOperatorValue(fightConditon.ComparisionOperator, EntityPercentHealth, fightConditon.SpecificValue);//TODO change to the character armor

                case FightConditionType.CharacterWithState:
                    return HasStatus(fightConditon.SpecificStatus);//TODO change to call on the chara

                case FightConditionType.CharacterWithoutState:
                    return !HasStatus(fightConditon.SpecificStatus);//TODO change to call on the chara

                case FightConditionType.MobArmor:
                    return ComparisonOperatorValue(fightConditon.ComparisionOperator, currentArmor, fightConditon.SpecificValue);

                case FightConditionType.MobsOnBoardWithState:
                    return HasStatus(fightConditon.SpecificStatus);//TODO change to the one mob

                case FightConditionType.MobWithState:
                    return HasStatus(fightConditon.SpecificStatus);

                case FightConditionType.MobWithoutState:
                    return !HasStatus(fightConditon.SpecificStatus);

                case FightConditionType.EachXTurns:
                    return 1 % fightConditon.SpecificValue == 0; //TODO change to the real number of turn

                case FightConditionType.MobPosition:
                    return ComparisonMobPosition(this, fightConditon.SpecificPosition); // TODO get the mob at the specific pos

                default:
                    Debug.LogError($"Default switch reach {fightConditon.ConditionType} for condition type");
                    return false;
            }
        }
    }
}