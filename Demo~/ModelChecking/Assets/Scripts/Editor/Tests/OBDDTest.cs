using System.IO;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public sealed class OBDDTest
{
    [Test]
    public void MakeLiteral()
    {
        var obdd = new OBDD();

        var a = obdd.MakeLiteral(0);
        var b = obdd.MakeLiteral(1);
        var c = obdd.MakeLiteral(2);
        Assert.IsNotNull(a);
        Assert.IsNotNull(b);
        Assert.IsNotNull(c);
        Assert.AreNotEqual(a, b);
        Assert.AreNotEqual(b, c);
        Assert.AreNotEqual(c, a);
    }

    [Test]
    public void MakeNot()
    {
        var obdd = new OBDD();

        var a = obdd.MakeLiteral(0);
        var b = obdd.MakeLiteral(1);
        Assert.IsNotNull(a);
        Assert.IsNotNull(b);
        Assert.AreNotEqual(a, b);

        var na = obdd.MakeNot(a);
        var nb = obdd.MakeNot(b);
        Assert.IsNotNull(na);
        Assert.IsNotNull(nb);
        Assert.AreNotEqual(na, nb);
        Assert.AreNotEqual(a, na);
        Assert.AreNotEqual(b, nb);

        var nna = obdd.MakeNot(na);
        var nnb = obdd.MakeNot(nb);
        Assert.IsNotNull(nna);
        Assert.IsNotNull(nnb);
        Assert.AreNotEqual(nna, nnb);

        // double negation cancels out
        Assert.AreEqual(a, nna);
        Assert.AreEqual(b, nnb);
    }

    [Test]
    public void Restrict()
    {
        var obdd = new OBDD();

        var a = obdd.MakeLiteral(0);
        var at = obdd.Restrict(a, 0, true);
        var af = obdd.Restrict(a, 0, false);

        Assert.AreEqual(BDDNode.True, at);
        Assert.AreEqual(BDDNode.False, af);
    }

    [Test]
    public void Example()
    {
        // f(a,b,c) = (!a & !b & !c) | (a & b) | (b & c)
        var obdd = new OBDD();

        var a = obdd.MakeNode(0, BDDNode.True, BDDNode.False);
        var b = obdd.MakeNode(1, BDDNode.True, BDDNode.False);
        var c = obdd.MakeNode(2, BDDNode.True, BDDNode.False);

        var na = obdd.MakeNot(a);
        var nb = obdd.MakeNot(b);
        var nc = obdd.MakeNot(c);

        var tmpAnd = obdd.MakeAnd(na, nb);
        var and1 = obdd.MakeAnd(tmpAnd, nc);
        var and2 = obdd.MakeAnd(a, b);
        var and3 = obdd.MakeAnd(b, c);

        var tmpOr = obdd.MakeOr(and1, and2);
        var or = obdd.MakeOr(tmpOr, and3);

        var root = or;

        using (var writer = new StringWriter())
        {
            root.ExportToGraphviz(writer);
            Debug.Log(writer);
        }
    }

    [Test]
    public void TransitionSystem()
    {
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

        var obdd = new OBDD();

        for (int i = 0; i < numNodes; i++)
        {
            for (int j = 0; j < numNodes; j++)
            {
                if (!ts.HasTransition(i, j)) continue;
                AddTransition(obdd, i, j);
            }
        }
    }

    [Test]
    public void AddTransition()
    {
        const int numNodes = 7;
        var ts = new TransitionSystem(numNodes);
    }

    private static BDDNode AddTransition(OBDD obdd, int i, int j, int numBits)
    {
        // example i = 4, j = 3, numBits = 4
        // binary i = 0100, j = 0011
        // interleave 00100101

        BDDNode prev = BDDNode.True;

        
        int bit = 0;
        while (j > 0)
        {
            bool value = (j & 1) == 1;

            var node = obdd.MakeLiteral(bit);
            if (!value)
            {
                node = obdd.MakeNot(node);
            }

            prev = obdd.MakeAnd(prev, node);
            bit += 2;
            j >>= 1;
        }

        bit = 1;
        while (i > 0)
        {
            bool value = (i & 1) == 1;

            var node = obdd.MakeLiteral(bit);
            if (!value)
            {
                node = obdd.MakeNot(node);
            }

            prev = obdd.MakeAnd(prev, node);
            bit += 2;
            i >>= 1;
        }

        return prev;
    }
}
