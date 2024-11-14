using OMG.Unit.Action;
using OMG.Unit.Monster;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Set Action", story: "Set [NextAction] To [Monster]", category: "Action", id: "135419fed12eac674a1da94576d7c1f1")]
public partial class SetActionAction : Action
{
    [SerializeReference] public BlackboardVariable<UnitAction> NextAction;
    [SerializeReference] public BlackboardVariable<Monster> Monster;

    protected override Status OnStart()
    {
        if (NextAction.Value == null || Monster.Value == null)
        {
            Debug.LogWarning($"Action : {NextAction.Value} or Barin : {Monster.Value} are equal to null");
            return Status.Failure;
        }

        Monster.Value.Brain.SetAction(NextAction.Value);
        return Status.Success;
    }

    protected override Status OnUpdate()
    {
        return Status.Running;
    }

    protected override void OnEnd() { }
}