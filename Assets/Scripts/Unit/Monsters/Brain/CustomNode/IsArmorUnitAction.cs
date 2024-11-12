using OMG.Unit;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Is Armor Unit", story: "Is [Unit] Armor [Comparison] [Value]", category: "Action", id: "d8db5f3ac000e3c2199d7dd917c41fe1")]
public partial class IsArmorUnitAction : Action
{
    [SerializeReference] public BlackboardVariable<UnitData> Unit;
    [SerializeReference] public BlackboardVariable<Comparison> Comparison;
    [SerializeReference] public BlackboardVariable<int> Value;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

