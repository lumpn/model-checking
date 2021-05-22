using System.Collections.Generic;

public sealed class Lookup
{
    private readonly Dictionary<string, int> identifiers = new Dictionary<string, int>();
    private readonly List<string> names = new List<string>();
    private int serial;

    public IEnumerable<string> VariableNames => names;

    public int Resolve(string identifier)
    {
        if (!identifiers.TryGetValue(identifier, out int id))
        {
            id = ResolveUnique(identifier);
            identifiers.Add(identifier, id);
        }

        return id;
    }

    public int ResolveUnique(string name)
    {
        names.Add(name);
        return serial++;
    }

    public override string ToString()
    {
        return $"({identifiers.Count}, {serial})";
    }
}
