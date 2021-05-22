public sealed class Consume : IScript
{
    private readonly int itemId, consumedId;
    private readonly string name;

    public Consume(Lookup lookup, string item, string name)
    {
        this.itemId = lookup.Resolve(item);
        this.consumedId = lookup.ResolveUnique($"-{item}");
        this.name = name;
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
        return name;
    }
}
