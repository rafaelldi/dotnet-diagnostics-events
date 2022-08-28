using System.Collections.Concurrent;
using System.Diagnostics;

namespace web_diagnostic_source;

public sealed class AllDiagnosticListenerObserver : IObserver<DiagnosticListener>
{
    private readonly ConcurrentDictionary<string, IDisposable> _subscription = new();

    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {
    }

    public void OnNext(DiagnosticListener listener)
    {
        Console.WriteLine($"Listener found: {listener.Name}");
        _subscription.TryAdd(listener.Name, listener.Subscribe(new CommonDiagnosticSourceObserver()));
        
        if (listener.Name == "Microsoft.Extensions.Hosting")
        {
            _subscription.TryAdd("CustomHostingObserver", listener.Subscribe(new MicrosoftHostingObserver()));
        }
    }
}