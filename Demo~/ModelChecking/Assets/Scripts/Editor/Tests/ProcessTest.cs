using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public sealed class ProcessTest
{
    [Test]
    public void KeyDoor()
    {
        var lookup = new Lookup();
        var key1 = new Acquire(lookup, "key", "key");
        var key2 = new Acquire(lookup, "key", "key");
        var door1 = new Consume(lookup, "key", "door");
        var door2 = new Consume(lookup, "key", "door");

        const int numNodes = 3;
        var process = new Process(numNodes);
        process.AddScript(0, key1);
        process.AddScript(0, key2); // removing this key fails EF(f)
        process.AddTransition(0, 1, door1);
        process.AddTransition(1, 2, door2); // changing this door to (0,1) passes EF(f) but not AG(EF(f))

        using (var writer = new StringWriter())
        {
            process.ExportToGraphviz(writer);
            Debug.Log(writer);
        }

        Debug.Log(lookup);
        var variables = lookup.VariableNames.ToArray();
        var ts = process.BuildTransitionSystem(variables, 0, 2, out int[] initialStates, out int[] finalStates, out IProposition[] propositions);

        var initialProposition = new Proposition(ts.numStates, "initial");
        initialProposition.Set(initialStates);

        var finalProposition = new Proposition(ts.numStates, "final");
        finalProposition.Set(finalStates);

        var allPropositions = new List<IProposition>(propositions);
        allPropositions.Add(initialProposition);
        allPropositions.Add(finalProposition);

        using (var writer = new StringWriter())
        {
            ts.ExportToGraphviz(writer, allPropositions.ToArray());
            Debug.Log(writer);
        }

        // EF(f) -- there exists a path starting at the initial states where f eventually holds (goal is reachable)
        var ef = new ExistsFutureProposition(ts.numStates, finalProposition);
        Assert.IsTrue(ef.Evaluate(ts, initialStates));

        // AG(EF(f)) -- from all reachable states there exists a path to the goal
        var ag = new AllGlobalProposition(ts.numStates, ef);
        Assert.IsTrue(ag.Evaluate(ts, initialStates));
    }
}
