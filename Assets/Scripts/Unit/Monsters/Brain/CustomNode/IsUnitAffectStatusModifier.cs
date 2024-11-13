using OMG.Unit;
using OMG.Unit.Status;
using System;
using Unity.Behavior;
using UnityEngine;
using Modifier = Unity.Behavior.Modifier;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Is Unit Affect Status", story: "Is [Unit] Affect By [StatusType]", category: "Flow/Conditional", id: "2380958d639fe9bd19f899904990a06b")]
public partial class IsUnitAffectStatusModifier : Modifier
{
    [SerializeReference] public BlackboardVariable<UnitData> Unit;
    [SerializeReference] public BlackboardVariable<StatusType> StatusType;

    protected override Status OnStart()
    {
        if (Unit.Value == null || Child == null)
        {
            Debug.LogWarning($"Unit : {Unit.Value} is equal to null");
            return Status.Failure;
        }

        if (Unit.Value.HaveStatus(StatusType.Value))
        {
            return StartNode(Child);
        }

        return Status.Failure;
    }

    protected override Status OnUpdate()
    {
        return Status.Failure;
    }

    protected override void OnEnd()
    {
    }
}