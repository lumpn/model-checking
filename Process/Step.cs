using System.Collections.Generic;

public sealed class Step
{
    public readonly int id;
    public readonly int node;
    public readonly State state;

    public readonly List<Step> successors = new List<Step>();
    public readonly List<Step> predecessors = new List<Step>();

    public Step(int id, int node, State state)
    {
        this.id = id;
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
