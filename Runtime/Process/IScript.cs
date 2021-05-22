public interface IScript
{
    /// <summary>execute the script on the specified state, returning the modified state or <c>null</c> if the script's preconditions are not met</summary>
    State Execute(State state);
}