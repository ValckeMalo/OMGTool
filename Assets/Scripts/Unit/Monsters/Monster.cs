namespace OMG.Unit.Monster
{
    using OMG.Unit;
    using OMG.Unit.Monster.Brain;

    public abstract class Monster : Unit
    {
        MonsterBrain brain;

        public abstract void Action();
        public abstract void SearchNextAction();
        public override abstract string GetName();
    }
}