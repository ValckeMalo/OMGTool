using OMG.Unit.Action;
using OMG.Unit.Monster.Brain;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Set Action", story: "Set [NextAction] To [MonsterBrain]", category: "Action", id: "135419fed12eac674a1da94576d7c1f1")]
public partial class SetActionAction : Action
{
    [SerializeReference] public BlackboardVariable<UnitAction> NextAction;
    [SerializeReference] public BlackboardVariable<MonsterBrain> MonsterBrain;

    protected override Status OnStart()
    {
        if (NextAction.Value == null || MonsterBrain.Value == null)
        {
            Debug.LogWarning($"Action : {NextAction.Value} or Barin : {MonsterBrain.Value} are equal to null");
            return Status.Failure;
        }

        MonsterBrain.Value.SetAction(NextAction.Value);
        return Status.Success;
    }

    protected override Status OnUpdate()
    {
        return Status.Failure;
    }

    protected override void OnEnd() { }
}