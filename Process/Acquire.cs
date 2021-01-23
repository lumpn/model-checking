public sealed class Acquire : IScript
{
    private int itemId, acquiredId;

    public Acquire(Lookup lookup, string item)
    {
        this.itemId = lookup.Resolve(item);
        this.acquiredId = lookup.ResolveUnique();
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
}
