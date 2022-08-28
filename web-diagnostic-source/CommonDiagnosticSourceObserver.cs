namespace web_diagnostic_source;

public sealed class CommonDiagnosticSourceObserver : IObserver<KeyValuePair<string, object?>>
{
    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {
    }

    public void OnNext(KeyValuePair<string, object?> value)
    {
        Console.WriteLine($"Event received: {value.Key}, {value.Value?.GetType()}");
    }
}