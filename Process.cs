using System;
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
        var numStates = CountStates(initialNode);

        var ts = new TransitionSystem(numStates);
        var pi = new Proposition(numStates, "initial");
        var pf = new Proposition(numStates, "final");
        initial = pi;
        final = pf;

        BuildTransitionSystem(initialNode, finalNode, ts, pi, pf);
        return ts;
    }

    private void BuildTransitionSystem(int initialNode, int finalNode, TransitionSystem ts, Proposition initial, Proposition final)
    {
        foreach (var transition in GetTransitions(initialNode))
        {

        }
    }

    public override string ToString()
    {
        return $"({numNodes}, {transitions.Count})";
    }
}
