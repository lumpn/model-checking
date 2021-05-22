public sealed class AllGlobalProposition : IProposition
{
    private readonly IProposition existsFuture, f;

    public AllGlobalProposition(int numStates, IProposition f)
    {
        this.f = f;

        // AG(f) === !EF(!f)
        var notF = new NotProposition(f);
        this.existsFuture = new ExistsFutureProposition(numStates, notF);
    }

    public bool Get(int state)
    {
        return !existsFuture.Get(state);
    }

    public bool Evaluate(TransitionSystem transitionSystem, int[] initialStates)
    {
        return !existsFuture.Evaluate(transitionSystem, initialStates);
    }

    public override string ToString()
    {
        return $"AG({f})";
    }
}
