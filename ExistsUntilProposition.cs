public sealed class ExistsUntilProposition : IProposition
{
    private readonly IProposition f, g;
    private readonly bool[] values;
    private bool isEvaluated;

    private int numNodes { get { return values.Length; } }

    public ExistsUntilProposition(int numNodes, IProposition f, IProposition g)
    {
        this.f = f;
        this.g = g;
        this.values = new bool[numNodes];
    }

    public bool Get(int node)
    {
        if (!isEvaluated) throw new System.InvalidOperationException($"{this} has not been evaluated yet.");
        return values[node];
    }

    private void Set(int node)
    {
        values[node] = true;
    }

    public bool Evaluate(TransitionSystem transitionSystem, IProposition initialStates)
    {
        isEvaluated = true;
        Evaluate(transitionSystem);
        return PropositionUtils.Evaluate(numNodes, this, initialStates);
    }

    private void Evaluate(TransitionSystem transitionSystem)
    {
        for (int i = 0; i < numNodes; i++)
        {
            if (g.Get(i))
            {
                Set(i);
                EvaluatePredecessors(transitionSystem, i);
            }
        }
    }

    private void EvaluatePredecessors(TransitionSystem transitionSystem, int node)
    {
        for (int i = 0; i < numNodes; i++)
        {
            if (transitionSystem.HasTransition(i, node))
            {
                Evaluate(transitionSystem, i);
            }
        }
    }

    private void Evaluate(TransitionSystem transitionSystem, int node)
    {
        if (Get(node))
        {
            // we have already evaluated this node
            return;
        }

        if (!f.Get(node))
        {
            // f does not hold
            return;
        }

        Set(node);
        EvaluatePredecessors(transitionSystem, node);
    }

    public override string ToString()
    {
        return $"E[{f} U {g}]";
    }
}
