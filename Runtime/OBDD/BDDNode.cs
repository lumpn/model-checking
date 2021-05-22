using System;

public sealed class BDDNode : IEquatable<BDDNode>
{
    public static readonly BDDNode True = new BDDNode(-1, null, null);
    public static readonly BDDNode False = new BDDNode(-1, null, null);

    public readonly int index; // variable identifier
    public readonly BDDNode high, low;

    public BDDNode(int index, BDDNode high, BDDNode low)
    {
        this.index = index;
        this.high = high;
        this.low = low;
    }

    public bool Equals(BDDNode other)
    {
        return index == other.index &&
               high == other.high &&
               low == other.low;
    }
}
