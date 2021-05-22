using System.Collections.Generic;
using System.IO;
using System.Linq;

/// <summary>a process is a transition system where each transition manipulates state</summary>
public sealed class Process
{
    private readonly int numNodes;
    private readonly List<Transition> transitions = new List<Transition>();

    public Process(int numNodes)
    {
        this.numNodes = numNodes;
    }

    /// <summary>adds the script to the node</summary>
    public void AddScript(int node, IScript script)
    {
        var transition = new Transition(node, node, script);
        transitions.Add(transition);
    }

    /// <summary>adds a bidirectional transition between the nodes</summary>
    public void AddTransition(int a, int b, IScript script)
    {
        var ta = new Transition(a, b, script);
        var tb = new Transition(b, a, script);
        transitions.Add(ta);
        transitions.Add(tb);
    }

    /// <summary>enumerates the state transitions of the process</summary>
    public TransitionSystem BuildTransitionSystem(string[] variables, int initialNode, int finalNode, out int[] intialStates, out int[] finalStates, out IProposition[] propositions)
    {
        // crawl
        var numVariables = variables.Length;
        var steps = new List<Step>();
        var initialState = new State(numVariables);
        var initialStep = Crawl(initialNode, initialState, steps);

        // create transition system and proposition
        var transitionSystem = new TransitionSystem(steps.Count);
        var props = new Proposition[numVariables];
        for (int i = 0; i < numVariables; i++)
        {
            props[i] = new Proposition(transitionSystem.numStates, variables[i]);
        }

        // convert steps to state transitions
        var addedSteps = new List<Step>();
        BuildTransitionSystem(transitionSystem, props, initialStep, addedSteps);

        // export initial and final states
        intialStates = new[] { initialStep.id };
        finalStates = steps.Where(p => p.node == finalNode).Select(p => p.id).ToArray();
        propositions = props;

        return transitionSystem;
    }

    /// <summary>adds all steps to the state transition system</summary>
    private void BuildTransitionSystem(TransitionSystem transitionSystem, Proposition[] propositions, Step step, List<Step> addedSteps)
    {
        // already added?
        if (FindStep(step.node, step.state, addedSteps) != null)
        {
            return;
        }

        addedSteps.Add(step);

        var state = step.state;
        for (int i = 0; i < state.numValues; i++)
        {
            if (state.Get(i))
            {
                // variable i is true in this state
                propositions[i].Set(step.id);
            }
        }

        // recurse
        foreach (var nextStep in step.Successors)
        {
            transitionSystem.AddTransition(step.id, nextStep.id);
            BuildTransitionSystem(transitionSystem, propositions, nextStep, addedSteps);
        }
    }

    /// <summary>crawls the process starting from the node and state</summary>
    private Step Crawl(int node, State state, List<Step> steps)
    {
        // already crawled?
        var existingStep = FindStep(node, state, steps);
        if (existingStep != null)
        {
            return existingStep;
        }

        var step = new Step(steps.Count, node, state);
        steps.Add(step);

        foreach (var transition in transitions)
        {
            // skip irrelevant transitions
            if (transition.source != node) continue;

            // try to execute script
            var nextState = transition.script.Execute(state);
            if (nextState == null) continue; // preconditions not met
            var nextNode = transition.target;

            // recurse
            var nextStep = Crawl(nextNode, nextState, steps);

            // link
            step.AddSuccessor(nextStep);
            nextStep.AddPredecessor(step);
        }

        return step;
    }

    /// <summary>searches an existing step with the node and state</summary>
    private Step FindStep(int node, State state, List<Step> steps)
    {
        foreach (var step in steps)
        {
            if (step.node != node) continue;
            if (!step.state.Equals(state)) continue;
            return step;
        }
        return null;
    }

    public void ExportToGraphviz(TextWriter writer)
    {
        writer.WriteLine("digraph G {");

        foreach (var transition in transitions)
        {
            writer.WriteLine("n{0} -> n{1} [label=\"{2}\"];", transition.source, transition.target, transition.script);
        }

        writer.WriteLine("}");
    }

    public override string ToString()
    {
        return $"({numNodes}, {transitions.Count})";
    }
}
