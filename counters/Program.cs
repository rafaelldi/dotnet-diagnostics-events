using counters;

Console.WriteLine("EventCounters sample application");
Console.WriteLine($"Current process id: {Environment.ProcessId}");

var listener = new MyEventCounterListener();
Task.Run(async () => await StartCounterProducingTask());

Console.ReadKey();

async Task StartCounterProducingTask()
{
    while (true)
    {
        MyEventCounterSource.Instance.Up();
        await Task.Delay(200);
    }
}