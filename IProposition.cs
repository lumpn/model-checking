public interface IProposition
{
    bool Get(int node);
    bool Evaluate(TransitionSystem transitionSystem, IProposition initialStates);
    string ToString();
}
