using NUnit.Framework;

[TestFixture]
public sealed class OBDDTest
{
    [Test]
    public void Foo()
    {
        //var ts = new TransitionSystem();
        //var propositions = new Proposition[4];

        //var obdd = OBDDUtils.Build(ts, propositions);
    }

    [Test]
    public void Example()
    {
        // f(a,b,c) = (!a & !b & !c) | (a & b) | (b & c)
        var obbd = new OBDD();

        var a = obbd.MakeNode(0, BDDNode.True, BDDNode.False);
        var b = obbd.MakeNode(1, BDDNode.True, BDDNode.False);
        var c = obbd.MakeNode(2, BDDNode.True, BDDNode.False);

        var na = obbd.MakeNot(a);
        var nb = obbd.MakeNot(b);
        var nc = obbd.MakeNot(c);

        var tmpAnd = obbd.MakeAnd(na, nb);
        var and1 = obbd.MakeAnd(tmpAnd, nc);
        var and2 = obbd.MakeAnd(a, b);
        var and3 = obbd.MakeAnd(b, c);

        var tmpOr = obbd.MakeOr(and1, and2);
        var or = obbd.MakeOr(tmpOr, and3);

        var root = or;
        // TODO Jonas: output to graphviz
    }
}
