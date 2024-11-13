using OMG.Unit;
using OMG.Unit.Status;
using System;
using Unity.Behavior;
using UnityEngine;
using Modifier = Unity.Behavior.Modifier;
using Unity.Properties;
using Unity.VisualScripting;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Is Unit Not Affect Status", story: "Is [Unit] Not Affect By [StatusType]", category: "Flow/Conditional", id: "181bb95cd7b94c604e2531b9b576d8f0")]
public partial class IsUnitNotAffectStatusModifier : Modifier
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

