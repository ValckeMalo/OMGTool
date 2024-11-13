using OMG.Unit;
using System;
using Unity.Behavior;
using UnityEngine;
using Modifier = Unity.Behavior.Modifier;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "If Unit Life ", story: "If [Unit] Life is [Comparison] [Value]", category: "Flow/Conditional", id: "5a15c5aa2e3ee8aab074872246bb5cf5")]
public partial class IfUnitLifeModifier : Modifier
{
    [SerializeReference] public BlackboardVariable<UnitData> Unit;
    [SerializeReference] public BlackboardVariable<Comparison> Comparison;
    [SerializeReference] public BlackboardVariable<int> Value;

    protected override Status OnStart()
    {
        if (Unit.Value == null || Child == null)
        {
            return LifeComparison(Unit.Value.hp, Value, Comparison) ? StartNode(Child) : Status.Failure;
        }

        return Status.Failure;
    }

    protected override Status OnUpdate()
    {
        return Status.Failure;
    }

    protected override void OnEnd() { }

    private bool LifeComparison(int value, int valueToCompare, Comparison comparison)
    {
        switch (comparison)
        {
            case global::Comparison.GreaterOrEqual:
                return value >= valueToCompare;

            case global::Comparison.Equal:
                return value == valueToCompare;

            case global::Comparison.LessOrEqual:
                return value <= valueToCompare;

            default:
                return false;
        }
    }
}