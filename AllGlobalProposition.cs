internal class AllGlobalProposition
{
    private readonly int numNodes;
    private readonly string name;
    private OrProposition or2;

    public AllGlobalProposition(int numNodes, string v, OrProposition or2)
    {
        this.numNodes = numNodes;
        this.v = v;
        this.or2 = or2;
    }
}