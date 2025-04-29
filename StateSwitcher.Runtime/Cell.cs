namespace StateSwitcher.Runtime;

/// <summary>
/// Represents a cell that can change its state based on various transition causes.
/// Implements the Actor model pattern for message processing.
/// </summary>
public class Cell : TypedActor<CellStateTransitionCause>
{
    /// <summary>
    /// The state machine that manages the cell's state transitions.
    /// </summary>
    StateMachine<CellState, CellStateTransitionCause> Machine { get; init; }

    /// <summary>
    /// Gets the current state of the cell.
    /// </summary>
    public CellState State { get { return Machine.CurrentState; } }

    /// <summary>
    /// Initializes a new cell with its state machine and defines all possible state transitions.
    /// </summary>
    public Cell()
    {
        Machine = new StateMachine<CellState, CellStateTransitionCause>(CellState.Open);

        // Define transitions from Open state
        Machine.AddTransition(CellState.Open, CellState.Close, CellStateTransitionCause.Filled, Precondition, Handler);

        // Define transitions from Close state
        Machine.AddTransition(CellState.Close, CellState.Open, CellStateTransitionCause.Empty, Precondition, Handler);
        Machine.AddTransition(CellState.Close, CellState.Open, CellStateTransitionCause.Broken, Precondition, Handler);
        Machine.AddTransition(CellState.Close, CellState.Open, CellStateTransitionCause.Hacked, Precondition, Handler);
        Machine.AddTransition(CellState.Close, CellState.Open, CellStateTransitionCause.Polluted, Precondition, Handler);

        // Define transitions that keep the cell in Open state
        Machine.AddTransition(CellState.Open, CellState.Open, CellStateTransitionCause.Booked, Precondition, Handler);
        Machine.AddTransition(CellState.Open, CellState.Open, CellStateTransitionCause.Fixed, Precondition, Handler);
    }

    /// <summary>
    /// Handles the transition by performing the associated action.
    /// </summary>
    /// <param name="cause">The cause of the state transition</param>
    void Handler(CellStateTransitionCause cause)
    {
        Console.WriteLine($"[{cause}] Do work...");
    }

    /// <summary>
    /// Processes incoming messages and triggers state transitions.
    /// </summary>
    /// <param name="cause">The cause of the state transition</param>
    protected override async Task HandleMessageAsync(CellStateTransitionCause cause)
    {
        Console.WriteLine($"Received: {cause}");
        
        Machine.TriggerCause(cause);

        await Task.CompletedTask;
    }

    /// <summary>
    /// Checks if the transition can be performed.
    /// </summary>
    /// <returns>True if the transition can be performed, false otherwise</returns>
    bool Precondition()
    {
        Console.WriteLine($"Check the precondition for the transition");
        return true;
    }
}
