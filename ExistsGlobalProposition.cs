public sealed class ExistsGlobalProposition : IProposition
{
    private readonly IProposition f;
    private readonly bool[] values;
    private bool isEvaluated;

    private int numStates { get { return values.Length; } }

    public ExistsGlobalProposition(int numStates, IProposition f)
    {
        this.f = f;
        this.values = new bool[numStates];
    }

    private void Set(int state)
    {
        values[state] = true;
    }

    public bool Get(int state)
    {
        if (!isEvaluated) throw new System.InvalidOperationException($"{this} has not been evaluated yet.");
        return values[state];
    }

    public bool Evaluate(TransitionSystem transitionSystem, int[] initialStates)
    {
        Evaluate(transitionSystem);
        return PropositionUtils.Evaluate(this, initialStates);
    }

    private void Evaluate(TransitionSystem transitionSystem)
    {
        isEvaluated = true;
        var checkedStates = new bool[numStates]; // already computed EG(f)
        var markedStates = new bool[numStates];  // in the process of computing EG(f)

        for (int i = 0; i < numStates; i++)
        {
            Evaluate(transitionSystem, checkedStates, markedStates, i);
        }
    }

    private bool Evaluate(TransitionSystem transitionSystem, bool[] checkedStates, bool[] markedStates, int state)
    {
        if (checkedStates[state])
        {
            // we have already evaluated this state
            return Get(state);
        }

        if (markedStates[state])
        {
            // we have reached a previously marked state
            // therefore detecting a cycle along which f holds
            return true;
        }

        if (!f.Get(state))
        {
            // f does not hold here (bad)
            return false;
        }

        // check if there is a path starting in the current state along which f holds for every state
        markedStates[state] = true;
        for (int i = 0; i < numStates; i++)
        {
            if (transitionSystem.HasTransition(state, i))
            {
                if (Evaluate(transitionSystem, checkedStates, markedStates, i))
                {
                    // found a path along which f holds for every state
                    // therefore prepend the current state
                    Set(state);
                    break;
                }
            }
        }
        markedStates[state] = false;
        checkedStates[state] = true;

        return Get(state);
    }

    public override string ToString()
    {
        return $"EG({f})";
    }
}
