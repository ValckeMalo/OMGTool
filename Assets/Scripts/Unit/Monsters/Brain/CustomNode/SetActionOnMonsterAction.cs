using OMG.Unit.Action;
using OMG.Unit.Monster.Brain;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetActionOnMonster", story: "Set [Action] on [Monster]", category: "Action", id: "9191984e07b7cec388cecb5d227e738f")]
public partial class SetActionOnMonsterAction : Action
{
    [SerializeReference] public BlackboardVariable<UnitAction> Action;
    [SerializeReference] public BlackboardVariable<MonsterBrain> Monster;

    protected override Status OnStart()
    {
        if (Action == null || Monster == null)
        {
            Debug.LogError($"{Action} or {Monster} are not set in the node");
            return Status.Failure;
        }

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Monster.Value.SetAction(Action.Value))
        {
            return Status.Success;
        }

        return Status.Failure;
    }

    protected override void OnEnd() { }
}

