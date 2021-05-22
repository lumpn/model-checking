using System.Linq;

public static class PropositionUtils
{
    public static bool Evaluate(IProposition proposition, int[] initialStates)
    {
        // proposition holds in all initial state
        return initialStates.All(proposition.Get);
    }
}
