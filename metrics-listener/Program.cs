using System.Diagnostics.Tracing;
using Microsoft.Diagnostics.NETCore.Client;
using Microsoft.Diagnostics.Tracing;

Console.WriteLine("Metrics listener sample application");
Console.WriteLine("Specify a target pid");
var input = Console.ReadLine();
if (!int.TryParse(input, out var pid))
{
    return;
}

var client = new DiagnosticsClient(pid);
var arguments = new Dictionary<string, string?>
{
    ["RefreshInterval"] = "1",
    ["Metrics"] = "Example.MyMeter\\my-counter"
};
var providers = new List<EventPipeProvider>
{
    new("System.Diagnostics.Metrics", EventLevel.Verbose, (long)EventKeywords.All, arguments)
};
using var session = client.StartEventPipeSession(providers, false);
using var source = new EventPipeEventSource(session.EventStream);

Console.CancelKeyPress += (_, eventArgs) =>
{
    eventArgs.Cancel = true;
    session.Stop();
};

source.Dynamic.All += HandleEvent;
source.Process();

static void HandleEvent(TraceEvent evt)
{
    if (evt.ProviderName != "System.Diagnostics.Metrics" || evt.EventName != "CounterRateValuePublished")
    {
        return;
    }

    var meter = (string)evt.PayloadValue(1);
    var counter = (string)evt.PayloadValue(3);
    var value = (string)evt.PayloadValue(6);

    Console.WriteLine($"{meter}/{counter} - {value}");
}