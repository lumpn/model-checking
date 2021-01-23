public sealed class ExistsGlobalProposition : IProposition
{
    private readonly IProposition f;
    private readonly bool[] values;

    public ExistsGlobalProposition(int numNodes, AndProposition f)
    {
        this.f = f;
        this.values = new bool[numNodes];
    }

    public bool Get(int node)
    {
        // TODO Jonas: make sure this proposition got evaluated
        return values[node];
    }

    public bool Evaluate(TransitionSystem transitionSystem, IProposition initialStates)
    {
        // TODO Jonas: implement
        return false;
    }
}
