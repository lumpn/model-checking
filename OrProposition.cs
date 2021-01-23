public sealed class OrProposition : IProposition
{
    private readonly Proposition f, g;

    public OrProposition(Proposition f, Proposition g)
    {
        this.f = f;
        this.g = g;
    }

    public bool Get(int node)
    {
        var a = f.Get(node);
        var b = g.Get(node);
        return (a || b);
    }

    public bool Evaluate(TransitionSystem transitionSystem, IProposition initialStates)
    {
        var a = f.Evaluate(transitionSystem, initialStates);
        var b = g.Evaluate(transitionSystem, initialStates);
        return (a || b);
    }

    public override string ToString()
    {
        return $"{f} || {g}";
    }
}
