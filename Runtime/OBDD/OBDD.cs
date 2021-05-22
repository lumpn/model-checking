using System;
using System.Collections.Generic;

public sealed class OBDD
{
    private readonly List<BDDNode> nodes = new List<BDDNode>();

    public BDDNode MakeNode(int index, BDDNode high, BDDNode low)
    {
        // node already exists?
        // TODO Jonas: this is supposed to be an O(1) hash lookup
        var newNode = new BDDNode(index, high, low);
        foreach (var node in nodes)
        {
            if (node.Equals(newNode))
            {
                return node;
            }
        }

        // register new node
        nodes.Add(newNode);
        return newNode;
    }

    /// <summary>restricts a BDD by assigning a value to a variable</summary>
    public BDDNode Restrict(BDDNode node, int variableId, bool variableValue)
    {
        if (node.index > variableId)
        {
            // our node and all our children do not contain the variable
            return node;
        }

        if (node.index < variableId)
        {
            // restrict children
            return MakeNode(node.index, Restrict(node.high, variableId, variableValue), Restrict(node.low, variableId, variableValue));
        }

        // restrict this node because it matches the variable
        var branch = variableValue ? node.high : node.low;
        return Restrict(branch, variableId, variableValue);
    }

    /// <summary>restricts an if-then-else block</summary>
    private BDDNode Restrict(BDDNode ifNode, BDDNode thenNode, BDDNode elseNode, int variableId, bool variableValue)
    {
        var i = Restrict(ifNode, variableId, variableValue);
        var t = Restrict(thenNode, variableId, variableValue);
        var e = Restrict(elseNode, variableId, variableValue);
        return MakeIfThenElse(i, t, e);
    }

    public BDDNode MakeIfThenElse(BDDNode ifNode, BDDNode thenNode, BDDNode elseNode)
    {
        // handle basic cases
        if (ifNode == BDDNode.True) return thenNode;
        if (ifNode == BDDNode.False) return elseNode;
        if (thenNode == BDDNode.True && elseNode == BDDNode.False) return ifNode;
        if (thenNode.Equals(elseNode)) return thenNode;

        // splitting variable must be the topmost root
        int splitId = Min(ifNode.index, thenNode.index, elseNode.index);

        // create restricted nodes for both cases
        var high = Restrict(ifNode, thenNode, elseNode, splitId, true);
        var low = Restrict(ifNode, thenNode, elseNode, splitId, false);
        return MakeNode(splitId, high, low);
    }

    public BDDNode MakeAnd(BDDNode a, BDDNode b)
    {
        return MakeIfThenElse(a, b, BDDNode.False);
    }

    public BDDNode MakeOr(BDDNode a, BDDNode b)
    {
        return MakeIfThenElse(a, BDDNode.True, b);
    }

    private static int Min(int a, int b, int c)
    {
        return Math.Min(Math.Min(a, b), c);
    }
}
