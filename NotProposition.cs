public sealed class NotProposition : IProposition
{
    private readonly Proposition inner;

    public NotProposition(Proposition inner)
    {
        this.inner = inner;
    }

    public bool Evaluate(TransitionSystem transitionSystem, IProposition initialStates)
    {
        return !inner.Evaluate(transitionSystem, initialStates);
    }

    public override string ToString()
    {
        return $"!{inner}";
    }
}
