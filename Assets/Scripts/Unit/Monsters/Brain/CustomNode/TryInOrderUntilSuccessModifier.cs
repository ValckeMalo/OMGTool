using System;
using Unity.Behavior;
using UnityEngine;
using Composite = Unity.Behavior.Composite;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Try In Order Until Success", category: "Flow", id: "b9396ee14f7d6ce9c4ff800a91c8123a")]
public partial class TryInOrderUntilSuccessModifier : Composite
{
    protected override Status OnStart()
    {
        if (Children.Count == 0)
            return Status.Success;

        int currentChild = 0;
        int childCount = Children.Count;

        Status status = Status.Uninitialized;
        while (status != Status.Success && currentChild < childCount)
        {
            status = StartNode(Children[currentChild]);
            currentChild++;
        }

        if (status == Status.Success)
            return Status.Success;

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

