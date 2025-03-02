namespace OMG.Game.Fight.Entities
{
    [System.Serializable]
    public class FightEntityStatus
    {
        private StatusType status;
        private int turn;

        public StatusType Status => status;
        public int Turn { get => turn; set => turn = value; }
        public void DecreaseTurn(int decreaseValue = -1)
        {
            turn -= decreaseValue;
        }

        public FightEntityStatus(StatusType status, int turn)
        {
            this.status = status;
            this.turn = turn;
        }
    }
}