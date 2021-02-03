using System.Collections.Generic;
using System;

public sealed class BDDNode : IEquatable<BDDNode>
{
    public readonly int index; // variable identifier
    public readonly BDDNode high, low;

    public static readonly BDDNode True = new BDDNode(-1, null, null);
    public static readonly BDDNode False = new BDDNode(-1, null, null);

    private static readonly List<BDDNode> nodes = new List<BDDNode>();

    public static BDDNode MakeNode(int index, BDDNode high, BDDNode low)
    {
        // TODO Jonas: this is supposed to be an O(1) hash lookup
        foreach (var node in nodes)
        {
            if (node.index == index && node.high == high && node.low == low)
            {
                return node;
            }
        }

        var newNode = new BDDNode(index, high, low);
        nodes.Add(newNode);
        return newNode;
    }

    private BDDNode(int index, BDDNode high, BDDNode low)
    {
        this.index = index;
        this.high = high;
        this.low = low;
    }

    public BDDNode Restrict(int var, bool val)
    {
        if (index > var)
        {
            return this;
        }

        if (index < var)
        {
            return MakeNode(index, high.Restrict(var, val), low.Restrict(var, val));
        }

        var branch = val ? high : low;
        return branch.Restrict(var, val);
    }

    public bool Equals(BDDNode other)
    {
        return (index == other.index
              && high == other.high
              && low  == other.low);
    }
}
