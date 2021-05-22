public sealed class ExistsFutureProposition : IProposition
{
    private readonly int numStates;
    private readonly IProposition f, existsUntil;

    public ExistsFutureProposition(int numStates, IProposition f)
    {
        this.numStates = numStates;
        this.f = f;

        // EF(f) === E[true U f]
        this.existsUntil = new ExistsUntilProposition(numStates, TrueProposition.Instance, f);
    }

    public bool Get(int state)
    {
        return existsUntil.Get(state);
    }

    public bool Evaluate(TransitionSystem transitionSystem, int[] initialStates)
    {
        return existsUntil.Evaluate(transitionSystem, initialStates);
    }

    public override string ToString()
    {
        return $"EF({f})";
    }
}
