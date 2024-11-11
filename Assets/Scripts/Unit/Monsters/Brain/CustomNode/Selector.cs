
namespace OMG.Unit.Monster.Brain.Node
{
    using MaloProduction.BehaviourTree;

    public class Selector : Composite
    {
        protected override Status OnStart()
        {
            if (children == null || children.Count <= 0)
            {
                return Status.Failure;
            }

            return Status.Running;
        }

        protected override void OnStop()
        {
            throw new System.NotImplementedException();
        }

        protected override Status OnUpdate()
        {
            foreach (Node child in children)
            {
                if (child.Update() == Status.Success)
                {
                    return Status.Success;
                }
            }

            return Status.Failure;
        }
    }
}