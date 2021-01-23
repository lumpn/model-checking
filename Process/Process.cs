using System.Collections.Generic;
using System.IO;

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

    public TransitionSystem BuildTransitionSystem(int numVariables, int initialNode, int finalNode, out IProposition initial, out IProposition final)
    {
        var steps = new List<Step>();
        var initialState = new State(numVariables);
        var initialStep = Crawl(initialNode, initialState, steps);

        var numSteps = steps.Count;
        var ts = new TransitionSystem(numSteps);

        var addedSteps = new List<Step>();
        AddSteps(ts, initialStep, addedSteps);

        var pi = new Proposition(numSteps, "initial");
        pi.Set(initialStep.id);

        var pf = new Proposition(numSteps, "final");
        foreach (var step in steps)
        {
            if (step.node != finalNode) continue;
            pf.Set(step.id);
        }

        initial = pi;
        final = pf;

        return ts;
    }

    private void AddSteps(TransitionSystem transitionSystem, Step step, List<Step> addedSteps)
    {
        if (FindStep(step.node, step.state, addedSteps) != null)
        {
            // already added
            return;
        }

        foreach (var nextStep in step.successors)
        {
            addedSteps.Add(nextStep);
            transitionSystem.AddTransition(step.id, nextStep.id);
            AddSteps(transitionSystem, nextStep, addedSteps);
        }
    }

    private Step Crawl(int node, State state, List<Step> steps)
    {
        var step = new Step(steps.Count, node, state);
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

    public void ExportToGraphviz(TextWriter writer)
    {
        writer.WriteLine("digraph G {");

        foreach (var transition in transitions)
        {
            writer.WriteLine($"n{transition.source} -> n{transition.target} [label=\"{transition.script}\"];");
        }

        writer.WriteLine("}");
    }

    public override string ToString()
    {
        return $"({numNodes}, {transitions.Count})";
    }
}
