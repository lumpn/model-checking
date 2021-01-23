internal class AndProposition
{
    private int numNodes;
    private string v;
    private Proposition s;
    private Proposition w;

    public AndProposition(int numNodes, string v, Proposition s, Proposition w)
    {
        this.numNodes = numNodes;
        this.v = v;
        this.s = s;
        this.w = w;
    }
}