using System.Collections.Generic;

public sealed class Lookup
{
    private readonly Dictionary<string, int> identifiers = new Dictionary<string, int>();
    private int serial;

    public int Resolve(string identifier)
    {
        if (!identifiers.TryGetValue(identifier, out int id))
        {
            id = ResolveUnique();
            identifiers.Add(identifier, id);
        }

        return id;
    }

    public int ResolveUnique()
    {
        return serial++;
    }
}
