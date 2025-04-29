namespace StateSwitcher.Runtime;

/// <summary>
/// Generic state machine implementation that manages state transitions based on causes.
/// </summary>
/// <typeparam name="S">The type of states (must be an enum)</typeparam>
/// <typeparam name="C">The type of causes that trigger state transitions</typeparam>
public class StateMachine<S, C> where S : struct, IConvertible
{
    private Dictionary<S, Dictionary<C, Transition<S, C>>> _states;
    /// <summary>
    /// Gets the current state of the state machine.
    /// </summary>
    public S CurrentState { get; private set; }

    /// <summary>
    /// Initializes a new state machine with the specified start state.
    /// </summary>
    /// <param name="startState">The initial state of the machine</param>
    /// <exception cref="ArgumentException">Thrown when S is not an enum type</exception>
    public StateMachine(S startState)
    {
        if (!typeof(S).IsEnum)
        {
            throw new ArgumentException(nameof(S));
        }

        _states = new Dictionary<S, Dictionary<C, Transition<S, C>>>();

        foreach (S state in Enum.GetValues(typeof(S)))
        {
            _states[state] = new Dictionary<C, Transition<S, C>>();
        }

        CurrentState = startState;
    }

    /// <summary>
    /// Adds a new transition to the state machine.
    /// </summary>
    /// <param name="fromState">The state from which the transition starts</param>
    /// <param name="toState">The state to which the transition leads</param>
    /// <param name="cause">The cause that triggers the transition</param>
    /// <param name="precondition">Function that determines if the transition can occur</param>
    /// <param name="action">Optional action to perform during the transition</param>
    /// <exception cref="ArgumentException">Thrown when fromState does not exist</exception>
    public void AddTransition(
        S fromState,
        S toState,
        C cause,
        Func<bool> precondition,
        Action<C> action = null)
    {
        if (!_states.ContainsKey(fromState))
        {
            throw new ArgumentException($"State {fromState} does not exist");
        }

        var transition = new Transition<S, C>(toState, cause, action, precondition);
        _states[fromState][cause] = transition;
    }

    /// <summary>
    /// Triggers a state transition based on the specified cause.
    /// </summary>
    /// <param name="cause">The cause that should trigger the transition</param>
    /// <exception cref="ArgumentException">Thrown when no transition exists for the current state and cause</exception>
    public void TriggerCause(C cause)
    {
        if (!_states[CurrentState].ContainsKey(cause))
        {
            throw new ArgumentException($"Has no transition for state {CurrentState} and cause {cause}");
        }

        var transition = _states[CurrentState][cause];
        CurrentState = transition.ToState;

        if (transition.Condition() == true && transition.Action != null)
        {
            transition.Action(cause);
        }
    }
}

