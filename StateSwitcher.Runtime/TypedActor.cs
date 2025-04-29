using System.Collections.Concurrent;

namespace StateSwitcher.Runtime;

public abstract class TypedActor<TMessage>
{
    private readonly ConcurrentQueue<TMessage> _mailbox = new ConcurrentQueue<TMessage>();
    private readonly Task _processingTask;
    private bool _isRunning = true;

    protected TypedActor()
    {
        _processingTask = Task.Run(ProcessMessagesAsync);
    }

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

    protected abstract Task HandleMessageAsync(TMessage message);

    public void SendMessage(TMessage message)
    {
        _mailbox.Enqueue(message);
    }

    public async Task StopAsync()
    {
        _isRunning = false;
        await _processingTask;
    }
}