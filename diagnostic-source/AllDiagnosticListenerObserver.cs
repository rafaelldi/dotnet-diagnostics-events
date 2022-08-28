using System.Diagnostics;

namespace diagnostic_source;

public sealed class AllDiagnosticListenerObserver : IObserver<DiagnosticListener>
{
    private const string MyDiagnosticSourceName = "Example.MyDiagnosticSource";
    private IDisposable? _myDiagnosticSourceSubscription;

    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {
    }

    public void OnNext(DiagnosticListener listener)
    {
        if (listener.Name == MyDiagnosticSourceName)
        {
            _myDiagnosticSourceSubscription ??= listener.Subscribe(new MyDiagnosticSourceObserver());
        }
    }
}