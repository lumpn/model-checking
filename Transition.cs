public class Transition
{
    public readonly int source, target;
    public readonly IScript script;

    public Transition(int source, int target, IScript script)
    {
        this.source = source;
        this.target = target;
        this.script = script;
    }
}
