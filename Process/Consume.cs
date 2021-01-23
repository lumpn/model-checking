public sealed class Consume : IScript
{
    private int itemId, consumedId;

    public Consume(Lookup lookup, string item)
    {
        this.itemId = lookup.Resolve(item);
        this.consumedId = lookup.ResolveUnique();
    }

    public State Execute(State state)
    {
        if (state.Get(consumedId)) return state; // already consumed
        if (!state.Get(itemId)) return null; // item not present

        var nextState = new State(state);
        nextState.Set(consumedId, true);
        nextState.Set(itemId, false);

        return nextState;
    }

    public override string ToString()
    {
        return $"-{itemId}";
    }
}
