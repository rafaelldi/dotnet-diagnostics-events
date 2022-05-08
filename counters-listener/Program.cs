using System.Diagnostics.Tracing;
using Microsoft.Diagnostics.NETCore.Client;
using Microsoft.Diagnostics.Tracing;

Console.WriteLine("EventCounters listener sample application");
Console.WriteLine("Specify a target pid");
var input = Console.ReadLine();
if (!int.TryParse(input, out var pid))
{
    return;
}

var client = new DiagnosticsClient(pid);
var arguments = new Dictionary<string, string?> { ["EventCounterIntervalSec"] = "1" };
var providers = new List<EventPipeProvider>
{
    new("Example.MyEventCounterSource", EventLevel.Verbose, (long)EventKeywords.All, arguments)
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
    if (evt.EventName != "EventCounters")
    {
        return;
    }

    if (evt.PayloadValue(0) is not IDictionary<string, object> payload)
    {
        return;
    }

    if (payload["Payload"] is not IDictionary<string, object> payloadFields)
    {
        return;
    }

    Console.WriteLine($"{payloadFields["DisplayName"]} - {payloadFields["Increment"]}");
}