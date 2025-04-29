namespace StateSwitcher.Runtime;

public class Program
{
    public static async Task Main()
    {
        Cell cell = new();


        Console.WriteLine($"Current state: {cell.State}");

        try
        {
            cell.SendMessage(CellStateTransitionCause.Booked);
            cell.SendMessage(CellStateTransitionCause.Filled);
            cell.SendMessage(CellStateTransitionCause.Hacked);
            cell.SendMessage(CellStateTransitionCause.Fixed);

            // cell.SendMessage(CellStateTransitionCause.Hacked); // throw Exception
            // Console.WriteLine($"Current state: {cell.State}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        await Task.Delay(1000);

        await cell.StopAsync();
    }
}

