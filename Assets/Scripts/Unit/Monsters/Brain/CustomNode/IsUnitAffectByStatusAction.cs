using OMG.Unit;
using OMG.Unit.Status;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Is Unit Affect by Status", story: "Is [Unit] Affect by [StatusType]", category: "Action", id: "3489934c0a58ef1cfc1868899c1fdf48")]
public partial class IsUnitAffectByStatusAction : Action
{
    [SerializeReference] public BlackboardVariable<UnitData> Unit;
    [SerializeReference] public BlackboardVariable<StatusType> StatusType;

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