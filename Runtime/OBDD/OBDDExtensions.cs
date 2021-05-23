public static class OBDDExtensions
{
    public static BDDNode MakeLiteral(this OBDD obdd, int index)
    {
        return obdd.MakeNode(index, BDDNode.True, BDDNode.False);
    }

    public static BDDNode MakeAnd(this OBDD obdd, BDDNode a, BDDNode b)
    {
        return obdd.MakeIfThenElse(a, b, BDDNode.False);
    }

    public static BDDNode MakeOr(this OBDD obdd, BDDNode a, BDDNode b)
    {
        return obdd.MakeIfThenElse(a, BDDNode.True, b);
    }

    public static BDDNode MakeNot(this OBDD obdd, BDDNode node)
    {
        return obdd.MakeIfThenElse(node, BDDNode.False, BDDNode.True);
    }
}
