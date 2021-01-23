using System.Collections.Generic;

public sealed class Process
{
    private readonly int numNodes;
    private readonly List<Transition> transitions = new List<Transition>();

    public Process(int numNodes)
    {
        this.numNodes = numNodes;
    }

    public void AddScript(int node, IScript script)
    {
        var transition = new Transition(node, node, script);
        transitions.Add(transition);
    }

    public void AddTransition(int a, int b, IScript script)
    {
        var ta = new Transition(a, b, script);
        var tb = new Transition(b, a, script);
        transitions.Add(ta);
        transitions.Add(tb);
    }

    public TransitionSystem BuildTransitionSystem(int initialNode, int finalNode, out IProposition initial, out IProposition final)
    {
        var steps = new List<Step>();
        var initialState = new State();
        var initialStep = Crawl(initialNode, initialState, steps);

        return ts;
    }

    private Step Crawl(int node, State state, List<Step> steps)
    {
        var step = new Step(node, state);
        steps.Add(step);

        foreach (var transition in transitions)
        {
            if (transition.source != node) continue;

            var nextState = transition.script.Execute(state);
            if (nextState == null) continue;

            var nextNode = transition.target;
            var nextStep = FindStep(nextNode, nextState, steps);
            if (nextStep == null)
            {
                nextStep = Crawl(nextNode, nextState, steps);
            }
            step.AddSuccessor(nextStep);
            nextStep.AddPredecessor(step);
        }

        return step;
    }

    private Step FindStep(int node, State state, List<Step> steps)
    {
        foreach (var step in steps)
        {
            if (step.node != node) continue;
            if (step.state != state) continue;
            return step;
        }
        return null;
    }

    public override string ToString()
    {
        return $"({numNodes}, {transitions.Count})";
    }
}
