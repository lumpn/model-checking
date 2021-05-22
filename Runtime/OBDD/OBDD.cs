using System;

public static class OBDD
{
    public static BDDNode IfThenElse(BDDNode i, BDDNode t, BDDNode e)
    {
        if (i == BDDNode.True) return t;
        if (i == BDDNode.False) return e;
        if (t == BDDNode.True && e == BDDNode.False) return i;
        if (t.Equals(e)) return t;

        // splitting variable must be the topmost root
        int splitVar = Math.Min(Math.Min(i.index, t.index), e.index);
        var ixt = i.Restrict(splitVar, true);
        var txt = t.Restrict(splitVar, true);
        var ext = e.Restrict(splitVar, true);
        var high = IfThenElse(ixt, txt, ext);

        var ixf = i.Restrict(splitVar, false);
        var txf = t.Restrict(splitVar, false);
        var exf = e.Restrict(splitVar, false);
        var low = IfThenElse(ixf, txf, exf);

        var result = BDDNode.MakeNode(splitVar, high, low);
        return result;
    }

    public static BDDNode And(BDDNode a, BDDNode b)
    {
        return IfThenElse(a, b, BDDNode.False);
    }

    public static BDDNode Or(BDDNode a, BDDNode b)
    {
        return IfThenElse(a, BDDNode.True, b);
    }
}
