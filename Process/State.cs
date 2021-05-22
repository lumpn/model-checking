using System;

/// <summary>a state is a collection of variables that can only be <c>true</c> or <c>false</c></summary>
public sealed class State : IEquatable<State>
{
    /// <summary>variable values</summary>
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

    public bool Get(int variableId)
    {
        return values[variableId];
    }

    public void Set(int variableId, bool value)
    {
        values[variableId] = value;
    }

    public bool Equals(State other)
    {
        for (int i = 0; i < numValues; i++)
        {
            var a = values[i];
            var b = other.values[i];
            if (a != b) return false;
        }
        return true;
    }
}
