public sealed class Proposition : PropositionBase
{
    public Proposition(int numNodes, string name)
        : base(numNodes, name)
    {
    }

    public override bool Evaluate(TransitionSystem transitionSystem, IProposition initialStates)
    {
        return PropositionUtils.Evaluate(Values.Length, this, initialStates);
    }
}
