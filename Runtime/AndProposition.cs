public sealed class AndProposition : IProposition
{
    private readonly IProposition f, g;

    public AndProposition(IProposition f, IProposition g)
    {
        this.f = f;
        this.g = g;
    }

    public bool Get(int state)
    {
        var a = f.Get(state);
        var b = g.Get(state);
        return (a && b);
    }

    public bool Evaluate(TransitionSystem transitionSystem, int[] initialStates)
    {
        var a = f.Evaluate(transitionSystem, initialStates);
        var b = g.Evaluate(transitionSystem, initialStates);
        return (a && b);
    }

    public override string ToString()
    {
        return $"({f} && {g})";
    }
}
