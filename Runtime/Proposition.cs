public sealed class Proposition : IProposition
{
    private readonly string name;
    private readonly bool[] values;

    public Proposition(int numStates, string name)
    {
        this.name = name;
        this.values = new bool[numStates];
    }

    /// <summary>set states in which the proposition holds</summary>
    public void Set(params int[] states)
    {
        foreach (var state in states)
        {
            values[state] = true;
        }
    }

    public bool Get(int state)
    {
        return values[state];
    }

    public bool Evaluate(TransitionSystem transitionSystem, int[] initialStates)
    {
        return PropositionUtils.Evaluate(this, initialStates);
    }

    public override string ToString()
    {
        return name;
    }
}
