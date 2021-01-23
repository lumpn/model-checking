public sealed class ExistsFutureProposition : IProposition
{
    private readonly Proposition inner;

    public ExistsFutureProposition(Proposition inner)
    {
        this.inner = inner;
    }

    public bool Evaluate(TransitionSystem transitionSystem, IProposition initialStates)
    {
        // EF(f) === E[true U f]
        var eu = new ExistsUntilProposition(TrueProposition.Instance, inner);
        return eu.Evaluate(transitionSystem, initialStates);
    }

    public override string ToString()
    {
        return $"EF({inner})";
    }
}
