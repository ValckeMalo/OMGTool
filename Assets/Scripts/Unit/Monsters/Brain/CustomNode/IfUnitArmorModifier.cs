using OMG.Unit;
using System;
using Unity.Behavior;
using UnityEngine;
using Modifier = Unity.Behavior.Modifier;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "If Unit Armor", story: "If [Unit] Armor is [Comparison] [Value]", category: "Action/Conditional", id: "c8b733c0b5f6320d113b9c29865dc9aa")]
public partial class IfUnitArmorModifier : Modifier
{
    [SerializeReference] public BlackboardVariable<Unit> Unit;
    [SerializeReference] public BlackboardVariable<Comparison> Comparison;
    [SerializeReference] public BlackboardVariable<int> Value;

    protected override Status OnStart()
    {
        if (Unit.Value == null || Child == null)
        {
            return LifeComparison(Unit.Value.Data.armor, Value, Comparison) ? StartNode(Child) : Status.Failure;
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