namespace OMG.Game.Fight.Entities
{
    using MVProduction.CustomAttributes;
    using OMG.Data.Character;
    using OMG.Data.Mobs.Actions;
    using OMG.Data.Mobs.Behaviour;
    using OMG.Data.Utils;

    using System;
    using System.Collections;
    using UnityEngine;

    [Serializable]
    public class FightMobEntity : FightEntity
    {
        [Title("Fight Mob Entity")]
        [SerializeField] private MobData mobData;
        [SerializeField, ReadOnly] private MobActionData nextMobAction;
        private Action<MobActionData> onNextActionUpdate = null;

        public void InitializeMob(int currentHealth, int maxHealth, MobData mobData, FightMobEnityUI mobEntityUI)
        {
            this.mobData = mobData;
            InitializeEntity(currentHealth, maxHealth, mobEntityUI);
            SearchNextAction();
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
            if (mobData == null || mobData.MobFightBehaviourList == null) return;

            //iterate throught all the behaviour of the mob and test all condition set to it and if true get the action from the list
            foreach (MobFightBehaviour behaviour in mobData.MobFightBehaviourList)
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

            if (nextMobAction.Target == FightEntityTarget.AllMobs)
                nextMobAction.ExecuteAction(FightManager.Instance.FightData.AllMobs.ToArray());
            else
                nextMobAction.ExecuteAction(GetTarget(nextMobAction.Target));

            nextMobAction = null;
        }
        private FightEntity GetTarget(FightEntityTarget target)
        {
            switch (target)
            {
                case FightEntityTarget.Me:
                    return this;

                case FightEntityTarget.Player:
                    return FightManager.Instance.FightData.FightCharacterEntity;

                case FightEntityTarget.FirstMob:
                    return FightManager.Instance.FightData.FirstFightMobEntity;

                case FightEntityTarget.LastMob:
                    return FightManager.Instance.FightData.LastFightMobEntity;

                case FightEntityTarget.RandomMob:
                    return FightManager.Instance.FightData.GetRandomMob();

                case FightEntityTarget.WeakestMob:
                    return FightManager.Instance.FightData.WeakestFightMobEntity();

                case FightEntityTarget.RandomMobOther:
                    return FightManager.Instance.FightData.GetRandomMob();//TODO set the pos of the current mob to execpt it for the random

                case FightEntityTarget.RandomMobWithState:
                    return FightManager.Instance.FightData.GetRandomMobWithStatus(Unit.Status.StatusType.Poison);

                case FightEntityTarget.RandomAll:
                    return FightManager.Instance.FightData.GetRandomEntity();

                case FightEntityTarget.AllMobs:
                default:
                    Debug.LogError($"Default switch reach {target} for target");
                    return null;
            }
        }

        private void SetNextAction(MobActionData nextMobAction)
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
                    return ComparisonOperatorValue(fightConditon.ComparisionOperator, FightManager.Instance.FightData.FightCharacterEntity.EntityPercentHealth, fightConditon.SpecificValue);

                case FightConditionType.MobsCount:
                    return ComparisonOperatorValue(fightConditon.ComparisionOperator, FightManager.Instance.FightData.MobCount, fightConditon.SpecificValue);

                case FightConditionType.FightTurn:
                    return ComparisonOperatorValue(fightConditon.ComparisionOperator, FightManager.Instance.FightData.NbTurn, fightConditon.SpecificValue);

                case FightConditionType.MobOnBoardWithHealth:
                    return ComparisonOperatorValue(fightConditon.ComparisionOperator, EntityPercentHealth, fightConditon.SpecificValue);//TODO change to search one mob

                case FightConditionType.CharacterArmor:
                    return ComparisonOperatorValue(fightConditon.ComparisionOperator, FightManager.Instance.FightData.FightCharacterEntity.Currentarmor, fightConditon.SpecificValue);

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