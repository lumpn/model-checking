using System.IO;
using System.Linq;

public sealed class TransitionSystem
{
    public readonly int numStates;
    private readonly bool[,] transitions;

    public TransitionSystem(int numStates)
    {
        this.numStates = numStates;
        this.transitions = new bool[numStates, numStates];
    }

    public void AddTransition(int sourceState, int targetState)
    {
        transitions[sourceState, targetState] = true;
    }

    public bool HasTransition(int sourceState, int targetState)
    {
        return transitions[sourceState, targetState];
    }

    public void ExportToGraphviz(TextWriter writer, IProposition[] propositions)
    {
        writer.WriteLine("digraph G {");

        for (int i = 0; i < numStates; i++)
        {
            var label = string.Join(", ", propositions.Where(p => p.Get(i)));
            writer.WriteLine("n{0} [label=\"{1}\"];", i, label);

            for (int j = 0; j < numStates; j++)
            {
                if (HasTransition(i, j))
                {
                    writer.WriteLine("n{0} -> n{1};", i, j);
                }
            }
        }

        writer.WriteLine("}");
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

        return $"({numStates}, {numTransitions})";
    }
}
