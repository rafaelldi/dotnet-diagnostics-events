namespace diagnostic_source;

public sealed class MyDiagnosticSourceObserver : IObserver<KeyValuePair<string, object?>>
{
    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {
    }

    public void OnNext(KeyValuePair<string, object?> value)
    {
        if (value.Key == MyEvent.Name)
        {
            var myEvent = (MyEvent?)value.Value;
            if (myEvent != null)
            {
                Console.WriteLine($"Event received: {myEvent.Value}");
            }
        }
    }
}