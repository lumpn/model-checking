/// <summary>a proposition that holds in certain states of a transition system</summary>
public interface IProposition
{
    /// <summary>whether the proposition holds in the specified state</summary>
    bool Get(int state);

    /// <summary>whether the proposition holds in all of the initial states</summary>
    bool Evaluate(TransitionSystem transitionSystem, int[] initialStates);

    /// <summary>human readable form</summary>
    string ToString();
}
