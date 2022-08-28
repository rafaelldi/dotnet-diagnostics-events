namespace web_diagnostic_source;

public sealed class MicrosoftHostingObserver : IObserver<KeyValuePair<string, object?>>
{
    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {
    }

    public void OnNext(KeyValuePair<string, object?> value)
    {
        if (value.Key == "HostBuilding")
        {
            var hostBuilder = (HostBuilder?)value.Value;
            var context = hostBuilder?.Properties[typeof(WebHostBuilderContext)];
            if (context is not WebHostBuilderContext webHostBuilderContext)
            {
                return;
            }
            
            Console.WriteLine($"My hosting environment: {webHostBuilderContext.HostingEnvironment.EnvironmentName}");
        }
    }
}