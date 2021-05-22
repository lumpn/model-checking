public sealed class TrueProposition : IProposition
{
    public static readonly TrueProposition Instance = new TrueProposition();

    public bool Get(int state)
    {
        return true;
    }

    public bool Evaluate(TransitionSystem transitionSystem, int[] initialStates)
    {
        return true;
    }

    public override string ToString()
    {
        return "true";
    }
}
