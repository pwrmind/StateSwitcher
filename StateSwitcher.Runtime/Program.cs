namespace StateSwitcher.Runtime;

/// <summary>
/// Main program class that demonstrates the usage of the Cell state machine.
/// </summary>
public class Program
{
    /// <summary>
    /// Entry point of the application that demonstrates state transitions in a Cell.
    /// </summary>
    public static async Task Main()
    {
        // Create a new cell instance
        Cell cell = new();

        // Display initial state
        Console.WriteLine($"Current state: {cell.State}");

        try
        {
            // Demonstrate various state transitions
            cell.SendMessage(CellStateTransitionCause.Booked);
            cell.SendMessage(CellStateTransitionCause.Filled);
            cell.SendMessage(CellStateTransitionCause.Hacked);
            cell.SendMessage(CellStateTransitionCause.Fixed);

            // Example of an invalid transition (commented out)
            // cell.SendMessage(CellStateTransitionCause.Hacked); // throw Exception
            // Console.WriteLine($"Current state: {cell.State}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        // Wait for a second to allow all messages to be processed
        await Task.Delay(1000);

        // Stop the cell's message processing
        await cell.StopAsync();
    }
}

