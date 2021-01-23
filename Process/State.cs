public sealed class State
{
    private readonly bool[] values;

    public State(int numValues)
    {
        this.values = new bool[numValues];
    }

    public State(State state)
    {
        this.values = (bool[])state.values.Clone();
    }

    public bool Get(int id)
    {
        return values[id];
    }

    public void Set(int id, bool value)
    {
        values[id] = value;
    }
}
