using OMG.Unit;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Is Life Unit", story: "Is [Unit] Life [Comparison] [Value]", category: "Action", id: "74b9726ac5700c8021e84eb487a0a648")]
public partial class IsLifeUnitAction : Action
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

