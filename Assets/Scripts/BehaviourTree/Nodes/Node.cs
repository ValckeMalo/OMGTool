namespace MaloProduction.BehaviourTree
{
    [System.Serializable]
    public abstract class Node
    {
        public enum Status
        {
            Uninitialized,
            Running,
            Success,
            Failure,
        }

        public Status CurrentStatus { get; protected set; } = Status.Uninitialized;

        protected abstract Status OnStart();
        protected abstract Status OnUpdate();
        protected abstract void OnStop();

        public Status Update()
        {
            if (CurrentStatus == Status.Uninitialized)
            {
                CurrentStatus = OnStart();

                if (CurrentStatus != Status.Running)
                {
                    return Status.Failure;
                }
            }

            Status status = OnUpdate();

            if (status == Status.Success || status == Status.Failure)
            {
                OnStop();
                CurrentStatus = Status.Uninitialized;
            }

            return status;
        }
    }
}