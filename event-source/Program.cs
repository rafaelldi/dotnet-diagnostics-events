using event_source;

Console.WriteLine("EventSource sample application");
Console.WriteLine($"Current process id: {Environment.ProcessId}");

var listener = new MyEventListener();
Task.Run(async () => await StartEventProducingTask());

Console.ReadKey();

async Task StartEventProducingTask()
{
    var random = new Random();
    while (true)
    {
        MyEventSource.Instance.ValueReceived(random.Next(0, 10));
        await Task.Delay(200);
    }
}