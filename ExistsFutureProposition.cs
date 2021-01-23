public sealed class ExistsFutureProposition : IProposition
{
    private readonly int numNodes;
    private readonly IProposition f, existsUntil;

    public ExistsFutureProposition(int numNodes, Proposition f)
    {
        this.numNodes = numNodes;
        this.f = f;

        // EF(f) === E[true U f]
        this.existsUntil = new ExistsUntilProposition(numNodes, TrueProposition.Instance, f);
    }

    public bool Get(int node)
    {
        return existsUntil.Get(node);
    }

    public bool Evaluate(TransitionSystem transitionSystem, IProposition initialStates)
    {
        return existsUntil.Evaluate(transitionSystem, initialStates);
    }

    public override string ToString()
    {
        return $"EF({f})";
    }
}
