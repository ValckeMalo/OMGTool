using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Invert Bool", story: "Invert [Bool]", category: "Action/Blackboard", id: "c1fa85d5de2cdcbf9a945950c0d329d1")]
public partial class InvertBoolAction : Action
{
    [SerializeReference] public BlackboardVariable<bool> Bool;

    protected override Status OnStart()
    {
        if (Bool == null) return Status.Failure;

        Bool.Value = !Bool.Value;
        return Status.Success;
    }
}