public sealed class TrueProposition : IProposition
{
    public static readonly TrueProposition Instance = new TrueProposition();

    public bool Evaluate(TransitionSystem transitionSystem, IProposition initialStates)
    {
        return true;
    }

    public override string ToString()
    {
        return "true";
    }
}
