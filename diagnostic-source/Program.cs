using System.Diagnostics;
using diagnostic_source;

Console.WriteLine("DiagnosticSource sample application");
Console.WriteLine($"Current process id: {Environment.ProcessId}");

const string myDiagnosticSourceName = "Example.MyDiagnosticSource";
DiagnosticSource diagnosticSource = new DiagnosticListener(myDiagnosticSourceName);

using var subscription = DiagnosticListener.AllListeners.Subscribe(new AllDiagnosticListenerObserver());

Task.Run(async () => await StartEventProducingTask());

Console.ReadKey();

async Task StartEventProducingTask()
{
    var random = new Random();
    while (true)
    {
        if (diagnosticSource.IsEnabled(MyEvent.Name))
        {
            diagnosticSource.Write(MyEvent.Name, new MyEvent(random.Next(0, 10)));
        }

        await Task.Delay(200);
    }
}