namespace OMG.Unit.Monster.Brain.Tree
{
    public abstract class Node
    {
        public abstract NodeResult Evaluate(IUnit player, IUnit monster, IUnit[] monsters);
    }
}