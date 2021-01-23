public sealed class ExistsUntilProposition : IProposition
{
    private readonly IProposition before, after;
    private readonly bool[] values;

    public ExistsUntilProposition(int numNodes, IProposition before, IProposition after)
    {
        this.before = before;
        this.after = after;
        this.values = new bool[numNodes];
    }

    public bool Get(int node)
    {
        // TODO Jonas: make sure this proposition got evaluated
        return values[node];
    }

    private void Set(int node)
    {
        values[node] = true;
    }

    public bool Evaluate(TransitionSystem transitionSystem, IProposition initialStates)
    {
        Evaluate(transitionSystem);

        var numNodes = values.Length;
        for (int i = 0; i < numNodes; i++)
        {
            if (initialStates.Get(i) && !Get(i))
            {
                // i is an initial state but the
                // proposition does not hold in i
                return false;
            }
        }
        return true;
    }

    private void Evaluate(TransitionSystem transitionSystem)
    {
        var numNodes = values.Length;
        for (int i = 0; i < numNodes; i++)
        {
            if (after.Get(i))
            {
                Set(i);
                for (int j = 0; j < numNodes; j++)
                {
                    if (transitionSystem.HasTransition(j, i))
                    {
                        Evaluate(transitionSystem, j);
                    }
                }
            }
        }
    }

    private void Evaluate(TransitionSystem transitionSystem, int node)
    {
        if (Get(node)) return; // already labeled
        if (!before.Get(node)) return; // before does not hold

        Set(node); // apply label

        for (int i = 0; i < numNodes; i++)
        {
            if (transitionSystem.HasTransition(i, node))
            {
                Evaluate(transitionSystem, i);
            }
        }
    }

    public override string ToString()
    {
        return $"E[{before} U {after}]";
    }
}
