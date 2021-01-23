public sealed class TransitionSystem
{
    private readonly int numNodes;
    private readonly bool[,] transitions;

    public TransitionSystem(int numNodes)
    {
        this.numNodes = numNodes;
        this.transitions = new bool[numNodes, numNodes];
    }

    public void AddTransition(int sourceNode, int targetNode)
    {
        transitions[sourceNode, targetNode] = true;
    }

    public bool HasTransition(int sourceNode, int targetNode)
    {
        return transitions[sourceNode, targetNode];
    }

    public override string ToString()
    {
        int numTransitions = 0;
        foreach (var transition in transitions)
        {
            if (transition)
            {
                numTransitions++;
            }
        }

        return $"({numNodes}, {numTransitions})";
    }
}
