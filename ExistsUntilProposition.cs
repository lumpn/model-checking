public sealed class ExistsUntilProposition : IProposition
{
    private readonly IProposition f, g;
    private readonly bool[] values;
    private bool isEvaluated;

    private int numStates { get { return values.Length; } }

    public ExistsUntilProposition(int numStates, IProposition f, IProposition g)
    {
        this.f = f;
        this.g = g;
        this.values = new bool[numStates];
    }

    public bool Get(int state)
    {
        if (!isEvaluated) throw new System.InvalidOperationException($"{this} has not been evaluated yet.");
        return values[state];
    }

    private void Set(int state)
    {
        values[state] = true;
    }

    public bool Evaluate(TransitionSystem transitionSystem, int[] initialStates)
    {
        Evaluate(transitionSystem);
        return PropositionUtils.Evaluate(this, initialStates);
    }

    private void Evaluate(TransitionSystem transitionSystem)
    {
        isEvaluated = true;
        for (int i = 0; i < numStates; i++)
        {
            if (g.Get(i))
            {
                Set(i);
                EvaluatePredecessors(transitionSystem, i);
            }
        }
    }

    private void EvaluatePredecessors(TransitionSystem transitionSystem, int state)
    {
        for (int i = 0; i < numStates; i++)
        {
            if (transitionSystem.HasTransition(i, state))
            {
                Evaluate(transitionSystem, i);
            }
        }
    }

    private void Evaluate(TransitionSystem transitionSystem, int state)
    {
        if (Get(state))
        {
            // we have already evaluated this state
            return;
        }

        if (!f.Get(state))
        {
            // f does not hold (bad)
            return;
        }

        // f holds in this state (good)
        Set(state);
        EvaluatePredecessors(transitionSystem, state);
    }

    public override string ToString()
    {
        return $"E[{f} U {g}]";
    }
}
