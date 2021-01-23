internal class OrProposition
{
    private int numNodes;
    private string v;
    private Proposition s;
    private Proposition w;

    public OrProposition(int numNodes, string v, Proposition s, Proposition w)
    {
        this.numNodes = numNodes;
        this.v = v;
        this.s = s;
        this.w = w;
    }
}