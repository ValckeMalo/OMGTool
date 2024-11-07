namespace OMG.Unit.Status
{
    [System.Serializable]
    public class UnitStatus
    {
        public StatusType status;
        public int turn;

        public UnitStatus(StatusType status, int turn)
        {
            this.status = status;
            this.turn = turn;
        }
    }
}