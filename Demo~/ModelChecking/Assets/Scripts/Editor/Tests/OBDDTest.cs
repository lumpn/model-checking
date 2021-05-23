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
}
