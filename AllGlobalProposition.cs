public sealed class AllGlobalProposition : IProposition
{
    private readonly IProposition existsFuture, f;

    public AllGlobalProposition(int numNodes, IProposition f)
    {
        this.f = f;

        // AG(f) === !EF(!f)
        var notF = new NotProposition(f);
        this.existsFuture = new ExistsFutureProposition(numNodes, notF);
    }

    public bool Get(int node)
    {
        return !existsFuture.Get(node);
    }

    public bool Evaluate(TransitionSystem transitionSystem, IProposition initialStates)
    {
        return !existsFuture.Evaluate(transitionSystem, initialStates);
    }

    public override string ToString()
    {
        return $"AG({f})";
    }
}
