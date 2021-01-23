using System;

public sealed class State : IComparable<State>
{
    private readonly bool[] values;

    public int numValues { get { return values.Length; } }

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

    public int CompareTo(State other)
    {
        for (int i = 0; i < numValues; i++)
        {
            var a = values[i];
            var b = other.values[i];
            var cmp = a.CompareTo(b);
            if (cmp != 0) return cmp;
        }
        return 0;
    }
}
