namespace StateSwitcher.Runtime;

/// <summary>
/// Represents a state transition in a state machine.
/// </summary>
/// <typeparam name="S">The type of states</typeparam>
/// <typeparam name="C">The type of causes</typeparam>
public class Transition<S, C>
{
    /// <summary>
    /// Gets the state to which this transition leads.
    /// </summary>
    public S ToState { get; }

    /// <summary>
    /// Gets the cause that triggers this transition.
    /// </summary>
    public C Cause { get; }

    /// <summary>
    /// Gets the action to perform during this transition.
    /// </summary>
    public Action<C> Action { get; }

    /// <summary>
    /// Gets the condition that must be met for this transition to occur.
    /// </summary>
    public Func<bool> Condition { get; }

    /// <summary>
    /// Initializes a new transition with the specified parameters.
    /// </summary>
    /// <param name="toState">The state to which this transition leads</param>
    /// <param name="cause">The cause that triggers this transition</param>
    /// <param name="action">The action to perform during this transition</param>
    /// <param name="condition">The condition that must be met for this transition to occur</param>
    public Transition(S toState, C cause, Action<C> action, Func<bool> condition)
    {
        ToState = toState;
        Cause = cause;
        Action = action;
        Condition = condition;
    }
}

