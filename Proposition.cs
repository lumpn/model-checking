public sealed class Proposition : IProposition
{
    private readonly string name;
    private readonly bool[] values;

    public Proposition(int numNodes, string name)
    {
        this.name = name;
        this.values = new bool[numNodes];
    }

    public void Set(params int[] nodes)
    {
        foreach (var node in nodes)
        {
            values[node] = true;
        }
    }

    public bool Get(int node)
    {
        return values[node];
    }

    public bool Evaluate(TransitionSystem transitionSystem, IProposition initialStates)
    {
        return PropositionUtils.Evaluate(values.Length, this, initialStates);
    }

    public override string ToString()
    {
        return name;
    }
}
