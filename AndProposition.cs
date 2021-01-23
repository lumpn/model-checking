public sealed class AndProposition : IProposition
{
    private readonly Proposition a, b;

    public AndProposition(Proposition a, Proposition b)
    {
        this.a = a;
        this.b = b;
    }

    public bool Evaluate(TransitionSystem transitionSystem, IProposition initialStates)
    {
        var resultA = a.Evaluate(transitionSystem, initialStates);
        var resultB = b.Evaluate(transitionSystem, initialStates);
        return (resultA && resultB);
    }

    public override string ToString()
    {
        return $"{a} && {b}";
    }
}
