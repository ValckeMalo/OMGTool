namespace OMG.Unit.Monster.Brain.Node
{
    using MaloProduction.BehaviourTree;
    using OMG.Unit.Action;

    public class SetAction : Action
    {
        public UnitAction action;
        public MonsterBrain target;

        protected override Status OnStart()
        {
            if (action == null || target == null)
            {
                return Status.Failure;
            }

            return Status.Running;
        }

        protected override void OnStop() { }

        protected override Status OnUpdate()
        {
            target.SetAction(action);

            return Status.Success;
        }
    }
}