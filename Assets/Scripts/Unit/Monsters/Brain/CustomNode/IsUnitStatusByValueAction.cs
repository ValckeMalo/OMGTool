using OMG.Unit;
using OMG.Unit.Status;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Is Unit Status By Value", story: "Is [Unit] Affect by [StatusType] [Comparison] [Value]", category: "Action", id: "52cbe751175f671d7f5db535f5be7a41")]
public partial class IsUnitStatusByValueAction : Action
{
    [SerializeReference] public BlackboardVariable<UnitData> Unit;
    [SerializeReference] public BlackboardVariable<StatusType> StatusType;
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

