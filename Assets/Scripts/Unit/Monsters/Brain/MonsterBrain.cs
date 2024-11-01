namespace OMG.Unit.Monster.Brain
{
    using OMG.Unit.Action;
    using OMG.Unit;
    using OMG.Unit.Monster.Brain.Tree;

    public abstract class MonsterBrain
    {
        protected RootNode brainRoot;
        protected UnitAction nextAction;
        protected System.Action<UnitAction> setter;

        public MonsterBrain()
        {
            setter = (action) => nextAction = action;
        }

        public virtual void ExecuteNextAction(IUnit player, IUnit monster, IUnit[] monsters)
        {
            nextAction.Execute(player, monster, monsters);
        }

        public virtual void SearchNextAction(IUnit player, IUnit monster, IUnit[] monsters)
        {
            brainRoot.Evaluate(player, monster, monsters);
        }
    }
}