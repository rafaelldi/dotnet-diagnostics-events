using System.Diagnostics.Tracing;
using Microsoft.Diagnostics.NETCore.Client;
using Microsoft.Diagnostics.Tracing;

Console.WriteLine("EventSource listener sample application");
Console.WriteLine("Specify a target pid");
var input = Console.ReadLine();
if (!int.TryParse(input, out var pid))
{
    return;
}

var client = new DiagnosticsClient(pid);
var providers = new List<EventPipeProvider>
{
    new("Example.MyEventSource", EventLevel.Verbose, (long)EventKeywords.All)
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
    if (evt.EventName != "ValueReceived")
    {
        return;
    }

    var eventValue = evt.PayloadValue(0);

    Console.WriteLine($"Event received: {eventValue}");
}