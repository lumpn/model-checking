using System.IO;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public sealed class ModelCheckingTest
{
    [Test]
    public void AlternatingBitProtocolSender()
    {
        // 0* --> 1  <-> 2        g*  --> s   <-> w
        // ^             |        ^               |
        // |             v        |               v
        // 5  <-> 4  <-- 3*       bw  <-> bs  <-- bg*

        const int numNodes = 6;
        var ts = new TransitionSystem(numNodes);
        ts.AddTransition(0, 0);
        ts.AddTransition(0, 1);
        ts.AddTransition(1, 2);
        ts.AddTransition(2, 1);
        ts.AddTransition(2, 3);
        ts.AddTransition(3, 3);
        ts.AddTransition(3, 4);
        ts.AddTransition(4, 5);
        ts.AddTransition(5, 4);
        ts.AddTransition(5, 0);

        var initial = new[] { 0, 3 };  // initial states

        var g = new Proposition(numNodes, "g"); // getting data from user
        g.Set(0, 3);

        var s = new Proposition(numNodes, "s"); // sending message
        s.Set(1, 4);

        var w = new Proposition(numNodes, "w"); // waiting for acknowledgement
        w.Set(2, 5);

        var b = new Proposition(numNodes, "b"); // control bit
        b.Set(3, 4, 5);

        using (var writer = new StringWriter())
        {
            ts.ExportToGraphviz(writer, new[] { g, s, w, b });
            Debug.Log(writer);
        }

        // AG(s || w || g) -- the sender is always in one of these three states
        var or1 = new OrProposition(s, w);
        var or2 = new OrProposition(or1, g);
        var ag = new AllGlobalProposition(numNodes, or2);
        Debug.Log(ag);
        Assert.IsTrue(ag.Evaluate(ts, initial));

        // EG(!s && !w)    -- there is a path where the sender is never sending nor waiting
        var notS = new NotProposition(s);
        var notW = new NotProposition(w);
        var and = new AndProposition(notS, notW);
        var eg = new ExistsGlobalProposition(numNodes, and);
        Debug.Log(eg);
        Assert.IsTrue(eg.Evaluate(ts, initial));
    }


    [Test]
    public void KeyDoor()
    {
        // process
        //
        // 0 <--door--> 1 <--door--> 2
        // key x2 

        // transition system
        //
        // 1 --> 3*
        // ^     |
        // |     v
        // 0     5 --> 6*
        // |     ^
        // v     |
        // 2 --> 4*

        const int numNodes = 7;
        var ts = new TransitionSystem(numNodes);
        ts.AddTransition(0, 1);
        ts.AddTransition(0, 2);
        ts.AddTransition(1, 3);
        ts.AddTransition(2, 4);
        ts.AddTransition(3, 3);
        ts.AddTransition(3, 5);
        ts.AddTransition(4, 4);
        ts.AddTransition(4, 5);
        ts.AddTransition(5, 5);
        ts.AddTransition(5, 6);
        ts.AddTransition(6, 6);

        var initial = new[] { 0 }; // initial states

        var a = new Proposition(numNodes, "a"); // has a key
        a.Set(1, 2, 5);

        var b = new Proposition(numNodes, "b"); //key1 acquired
        b.Set(1, 3, 5, 6);

        var c = new Proposition(numNodes, "c"); // key2 acquired
        c.Set(2, 4, 5, 6);

        var d = new Proposition(numNodes, "d"); // door1 open
        d.Set(3, 4, 5, 6);

        var e = new Proposition(numNodes, "e"); //door2 open
        e.Set(6);

        var f = new Proposition(numNodes, "f"); // goal
        f.Set(6);

        using (var writer = new StringWriter())
        {
            ts.ExportToGraphviz(writer, new[] { a, b, c, d, e, f });
            Debug.Log(writer);
        }

        // EF(f)     -- there exists a path starting at the initial states where f eventually holds (goal is reachable)
        var ef = new ExistsFutureProposition(numNodes, f);
        Debug.Log(ef);
        Assert.IsTrue(ef.Evaluate(ts, initial));

        // AG(EF(f)) -- from all reachable states there exists a path to the goal
        var ag = new AllGlobalProposition(numNodes, ef);
        Debug.Log(ag);
        Assert.IsTrue(ag.Evaluate(ts, initial));
    }
}
