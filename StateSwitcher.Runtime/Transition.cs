namespace StateSwitcher.Runtime;

public class Transition<S, C>
{
    public S ToState { get; }
    public C Cause { get; }
    public Action<C> Action { get; }
    public Func<bool> Condition { get; }

    public Transition(S toState, C cause, Action<C> action, Func<bool> condition)
    {
        ToState = toState;
        Cause = cause;
        Action = action;
        Condition = condition;
    }
}

