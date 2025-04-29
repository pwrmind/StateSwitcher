namespace StateSwitcher.Runtime;

public class Cell : TypedActor<CellStateTransitionCause>
{
    StateMachine<CellState, CellStateTransitionCause> Machine { get; init; }

    public CellState State { get { return Machine.CurrentState; } }

    public Cell()
    {
        Machine = new StateMachine<CellState, CellStateTransitionCause>(CellState.Open);

        Machine.AddTransition(CellState.Open, CellState.Close, CellStateTransitionCause.Filled, Precondition, Handler);

        Machine.AddTransition(CellState.Close, CellState.Open, CellStateTransitionCause.Empty, Precondition, Handler);
        Machine.AddTransition(CellState.Close, CellState.Open, CellStateTransitionCause.Broken, Precondition, Handler);
        Machine.AddTransition(CellState.Close, CellState.Open, CellStateTransitionCause.Hacked, Precondition, Handler);
        Machine.AddTransition(CellState.Close, CellState.Open, CellStateTransitionCause.Polluted, Precondition, Handler);

        Machine.AddTransition(CellState.Open, CellState.Open, CellStateTransitionCause.Booked, Precondition, Handler);
        Machine.AddTransition(CellState.Open, CellState.Open, CellStateTransitionCause.Fixed, Precondition, Handler);
    }

    void Handler(CellStateTransitionCause cause)
    {
        Console.WriteLine($"[{cause}] Do work...");
    }

    protected override async Task HandleMessageAsync(CellStateTransitionCause cause)
    {
        Console.WriteLine($"Received: {cause}");
        
        Machine.TriggerCause(cause);

        await Task.CompletedTask;
    }

    bool Precondition()
    {
        Console.WriteLine($"Check the precondition for the transition");

        return true;
    }
}
