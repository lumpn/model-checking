using System;

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

    private bool Evaluate(TransitionSystem transitionSystem, bool[] checkedNodes, bool[] markedNodes, int s)
    {
        if (!checkedNodes[s])
        {
            if (markedNodes[s])
            {
                // DFS has reached a marked node
                // therefore detecting an infinite cycle
                // along which f holds
                return true;
            }

            if (f.Get(s))
            {
                markedNodes[s] = true;
                for (int s2 = 0; s2 < numNodes; s2++)
                {
                    if (transitionSystem.HasTransition(s, s2))
                    {
                        if (Evaluate(transitionSystem, checkedNodes, markedNodes, s2))
                        {
                            // found an infinite cycle along which f holds
                            // therefore it also holds in the starting node
                            Set(s);
                            checkedNodes[s] = true;
                            return true;
                        }
                    }
                }
                markedNodes[s] = false;
            }
            checkedNodes[s] = true;
        }

        // we have already computed the result for this node
        return Get(s);
    }

    public override string ToString()
    {
        return $"EG({f})";
    }
}
