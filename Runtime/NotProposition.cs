public sealed class NotProposition : IProposition
{
    private readonly IProposition f;

    public NotProposition(IProposition f)
    {
        this.f = f;
    }

    public bool Get(int state)
    {
        return !f.Get(state);
    }

    public bool Evaluate(TransitionSystem transitionSystem, int[] initialStates)
    {
        return !f.Evaluate(transitionSystem, initialStates);
    }

    public override string ToString()
    {
        return $"!{f}";
    }
}
