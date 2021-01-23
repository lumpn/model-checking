public sealed class NotProposition : IProposition
{
    private readonly IProposition f;

    public NotProposition(IProposition f)
    {
        this.f = f;
    }

    public bool Get(int node)
    {
        return !f.Get(node);
    }

    public bool Evaluate(TransitionSystem transitionSystem, IProposition initialStates)
    {
        return !f.Evaluate(transitionSystem, initialStates);
    }

    public override string ToString()
    {
        return $"!{f}";
    }
}
