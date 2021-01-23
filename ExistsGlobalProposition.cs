public sealed class ExistsGlobalProposition : IProposition
{
    private readonly IProposition f;
    private readonly bool[] values;

    private int numNodes { get { return values.Length; } }

    public ExistsGlobalProposition(int numNodes, AndProposition f)
    {
        this.f = f;
        this.values = new bool[numNodes];
    }

    private void Set(int node)
    {
        values[node] = true;
    }

    public bool Get(int node)
    {
        // TODO Jonas: make sure this proposition got evaluated
        return values[node];
    }

    public bool Evaluate(TransitionSystem transitionSystem, IProposition initialStates)
    {
        Evaluate(transitionSystem);
        return PropositionUtils.Evaluate(numNodes, this, initialStates);
    }

    private void Evaluate(TransitionSystem transitionSystem)
    {
        var checkedNodes = new bool[numNodes]; // already computed EG(f)
        var markedNodes = new bool[numNodes];  // in the process of computing EG(f)

        for (int i = 0; i < numNodes; i++)
        {
            Evaluate(transitionSystem, checkedNodes, markedNodes, i);
        }
    }

    private bool Evaluate(TransitionSystem transitionSystem, bool[] checkedNodes, bool[] markedNodes, int node)
    {
        if (checkedNodes[node])
        {
            // we have already evaluated this node
            return Get(node);
        }

        if (markedNodes[node])
        {
            // we have reached a previously marked node
            // therefore detecting a cycle along which f holds
            return true;
        }

        if (f.Get(node))
        {
            // proposition does not hold here
            return false;
        }

        // check if there is a path starting in the current along which f holds for every node
        markedNodes[node] = true;
        for (int i = 0; i < numNodes; i++)
        {
            if (transitionSystem.HasTransition(node, i))
            {
                if (Evaluate(transitionSystem, checkedNodes, markedNodes, i))
                {
                    // found a path along which f holds for every node
                    // therefore prepend the current node
                    Set(node);
                    break;
                }
            }
        }
        markedNodes[node] = false;
        checkedNodes[node] = true;

        return Get(node);
    }

    public override string ToString()
    {
        return $"EG({f})";
    }
}
