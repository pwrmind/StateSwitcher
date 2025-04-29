using System.Collections.Concurrent;

namespace StateSwitcher.Runtime;

/// <summary>
/// Abstract base class for implementing the Actor model pattern with typed messages.
/// Provides message queueing and processing functionality.
/// </summary>
/// <typeparam name="TMessage">The type of messages this actor can process</typeparam>
public abstract class TypedActor<TMessage>
{
    private readonly ConcurrentQueue<TMessage> _mailbox = new ConcurrentQueue<TMessage>();
    private readonly Task _processingTask;
    private bool _isRunning = true;

    /// <summary>
    /// Initializes a new instance of TypedActor and starts the message processing task.
    /// </summary>
    protected TypedActor()
    {
        _processingTask = Task.Run(ProcessMessagesAsync);
    }

    /// <summary>
    /// Continuously processes messages from the mailbox until the actor is stopped.
    /// </summary>
    private async Task ProcessMessagesAsync()
    {
        while (_isRunning)
        {
            if (_mailbox.TryDequeue(out var message))
            {
                await HandleMessageAsync(message);
            }
            else
            {
                await Task.Delay(10);
            }
        }
    }

    /// <summary>
    /// Abstract method to be implemented by derived classes to handle specific message types.
    /// </summary>
    /// <param name="message">The message to handle</param>
    protected abstract Task HandleMessageAsync(TMessage message);

    /// <summary>
    /// Enqueues a message to the actor's mailbox for processing.
    /// </summary>
    /// <param name="message">The message to enqueue</param>
    public void SendMessage(TMessage message)
    {
        _mailbox.Enqueue(message);
    }

    /// <summary>
    /// Stops the actor's message processing and waits for the current task to complete.
    /// </summary>
    public async Task StopAsync()
    {
        _isRunning = false;
        await _processingTask;
    }
}