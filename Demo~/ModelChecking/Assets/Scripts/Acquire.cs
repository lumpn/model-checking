public sealed class Acquire : IScript
{
    private readonly int itemId, acquiredId;
    private readonly string name;

    public Acquire(Lookup lookup, string item, string name)
    {
        this.itemId = lookup.Resolve(item);
        this.acquiredId = lookup.ResolveUnique($"+{item}");
        this.name = name;
    }

    public State Execute(State state)
    {
        if (state.Get(acquiredId)) return state; // already acquired
        if (state.Get(itemId)) return null; // item already present

        var nextState = new State(state);
        nextState.Set(acquiredId, true);
        nextState.Set(itemId, true);

        return nextState;
    }

    public override string ToString()
    {
        return name;
    }
}
