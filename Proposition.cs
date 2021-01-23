public sealed class Proposition : PropositionBase
{
    public Proposition(int numNodes, string name)
        : base(numNodes, name)
    {
    }

    public override bool Evaluate(TransitionSystem transitionSystem, IProposition initialStates)
    {
        var values = initialStates.Values;
        for (int i = 0; i < values.Length; i++)
        {
            var value = values[i];
            if (value) // i is an initial state
            {
                if (!Get(i)) // proposition does not hold in i
                {
                    return false;
                }
            }
        }
        return true; // proposition holds for all initial states
    }
}
