public static class PropositionUtils
{
    public static bool Evaluate(int numNodes, IProposition proposition, IProposition initialStates)
    {
        for (int i = 0; i < numNodes; i++)
        {
            if (initialStates.Get(i) && !proposition.Get(i))
            {
                // i is an initial state but the
                // proposition does not hold in i
                return false;
            }
        }
        return true;
    }
}
