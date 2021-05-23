using System.IO;
using System.Linq;
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
    public void AddTransition()
    {
        const int numBits = 3;
        var obdd = new OBDD();
        var root = AddTransition(obdd, 0, 1, numBits);

        // example i = 0, j = 1, numBits = 3
        // binary i = 000, j = 001
        // interleave j2 i2 j1 i1 j0 i0
        //            0  0  0  0  1  0
        // variables  5  4  3  2  1  0

        using (var writer = new StringWriter())
        {
            root.ExportToGraphviz(writer);
            Debug.Log(writer);
        }

        using (var writer = new StringWriter())
        {
            obdd.ExportToGraphviz(writer);
            Debug.Log(writer);
        }
    }

    [Test]
    public void TransitionSystem()
    {
        const int numNodes = 7;
        const int numBits = 3;
        Assert.Less(numNodes, 1 << (numBits + 1));

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
        var root = BDDNode.False;

        for (int i = 0; i < numNodes; i++)
        {
            for (int j = 0; j < numNodes; j++)
            {
                if (!ts.HasTransition(i, j)) continue;
                var transition = AddTransition(obdd, i, j, numBits);
                root = obdd.MakeOr(root, transition);
            }
        }

        using (var writer = new StringWriter())
        {
            ts.ExportToGraphviz(writer, new IProposition[0]);
            Debug.Log(writer);
        }

        using (var writer = new StringWriter())
        {
            root.ExportToGraphviz(writer);
            Debug.Log(writer);
        }

        using (var writer = new StringWriter())
        {
            obdd.ExportToGraphviz(writer);
            Debug.Log(writer);
        }
    }

    private static BDDNode AddTransition(OBDD obdd, int i, int j, int numBits)
    {
        var node = BDDNode.True;
        node = AddTransition(obdd, node, i, numBits, 0);
        node = AddTransition(obdd, node, j, numBits, 1);
        return node;
    }

    private static BDDNode AddTransition(OBDD obdd, BDDNode node, int index, int numBits, int offset)
    {
        for (int i = 0; i < numBits; i++)
        {
            int literal = i * 2 + offset;
            var variable = obdd.MakeLiteral(literal);

            bool hasBit = (index & (1 << i)) != 0;
            if (!hasBit)
            {
                variable = obdd.MakeNot(variable);
            }

            node = obdd.MakeAnd(node, variable);
        }

        return node;
    }

    private static bool HasFlag(int a, int b)
    {
        return (a & b) == b;
    }
}
