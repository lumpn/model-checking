public abstract class PropositionBase : IProposition
{
    private readonly string name;
    private readonly bool[] values;

    public string Name { get { return name; } }
    public bool[] Values { get { return values; } }

    public PropositionBase(int numNodes, string name)
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

    public override string ToString()
    {
        return name;
    }

    public abstract bool Evaluate(TransitionSystem transitionSystem, IProposition initialStates);
}
