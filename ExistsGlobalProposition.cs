internal class ExistsGlobalProposition
{
    private int numNodes;
    private string v;
    private AndProposition and;

    public ExistsGlobalProposition(int numNodes, string v, AndProposition and)
    {
        this.numNodes = numNodes;
        this.v = v;
        this.and = and;
    }
}