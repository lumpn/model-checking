using System.Collections.Generic;

public sealed class Step
{
    public readonly int node;
    public readonly State state;

    private readonly List<Step> successors = new List<Step>();
    private readonly List<Step> predecessors = new List<Step>();

    public Step(int node, State state)
    {
        this.node = node;
        this.state = state;
    }

    public void AddSuccessor(Step step)
    {
        successors.Add(step);
    }

    public void AddPredecessor(Step step)
    {
        predecessors.Add(step);
    }
}
