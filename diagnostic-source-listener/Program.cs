using System.Diagnostics.Tracing;
using Microsoft.Diagnostics.NETCore.Client;
using Microsoft.Diagnostics.Tracing;

Console.WriteLine("DiagnosticSource listener sample application");
Console.WriteLine("Specify a target pid");
var input = Console.ReadLine();
if (!int.TryParse(input, out var pid))
{
    return;
}

var client = new DiagnosticsClient(pid);
var arguments = new Dictionary<string, string?> { ["FilterAndPayloadSpecs"] = "Example.MyDiagnosticSource/MyEvent:-Value" };
var providers = new List<EventPipeProvider>
{
    new("Microsoft-Diagnostics-DiagnosticSource", EventLevel.Verbose, (long)EventKeywords.All, arguments)
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
    if (evt.EventName != "Event")
    {
        return;
    }

    var sourceName = evt.PayloadValue(0) as string;
    var eventName = evt.PayloadValue(1) as string;
    if (sourceName != "Example.MyDiagnosticSource" || eventName != "MyEvent")
    {
        return;
    }
    
    if (evt.PayloadValue(2) is not IDictionary<string, object>[] payload)
    {
        return;
    }

    var eventValue = payload[0]["Value"];

    Console.WriteLine($"Event received: {eventValue}");
}
