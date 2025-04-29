namespace StateSwitcher.Runtime;

public class StateMachine<S, C> where S : struct, IConvertible
{
    private Dictionary<S, Dictionary<C, Transition<S, C>>> _states;
    public S CurrentState { get; private set; }

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

