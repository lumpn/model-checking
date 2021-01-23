public interface IProposition
{
    bool Evaluate(TransitionSystem transitionSystem, IProposition initialStates);
    string ToString();
}
