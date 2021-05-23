using System;
using System.IO;
using System.Collections.Generic;

public sealed class BDDNode
{
    public static readonly BDDNode True = new BDDNode(int.MaxValue - 1, int.MaxValue - 1, null, null);
    public static readonly BDDNode False = new BDDNode(int.MaxValue - 2, int.MaxValue - 2, null, null);

    public readonly int id;
    public readonly int variableId;
    public readonly BDDNode high, low;

    public BDDNode(int id, int variableId, BDDNode high, BDDNode low)
    {
        this.id = id;
        this.variableId = variableId;
        this.high = high;
        this.low = low;
    }

    public bool IsEquivalent(BDDNode other)
    {
        return variableId == other.variableId &&
               high == other.high &&
               low == other.low;
    }

    public void ExportToGraphviz(TextWriter writer)
    {
        writer.WriteLine("digraph G {");

        var exported = new HashSet<BDDNode>();
        exported.Add(True);
        exported.Add(False);

        writer.WriteLine("n{0} [label=\"true\"];", True.id);
        writer.WriteLine("n{0} [label=\"false\"];", False.id);

        var stack = new Stack<BDDNode>();
        stack.Push(this);

        while (stack.Count > 0)
        {
            var node = stack.Pop();
            if (!exported.Add(node)) continue;

            writer.WriteLine("n{0} [label=\"{1}\"];", node.id, node.variableId);
            writer.WriteLine("n{0} -> n{1};", node.id, node.high.id);
            writer.WriteLine("n{0} -> n{1} [style=dashed];", node.id, node.low.id);

            stack.Push(node.high);
            stack.Push(node.low);
        }

        writer.WriteLine("}");
    }

    public override string ToString()
    {
        return $"({id}, {variableId}, {high?.id}, {low?.id})";
    }
}
