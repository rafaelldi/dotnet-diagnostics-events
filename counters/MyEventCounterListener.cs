using System.Diagnostics.Tracing;

namespace counters;

public sealed class MyEventCounterListener : EventListener
{
    protected override void OnEventSourceCreated(EventSource eventSource)
    {
        if (eventSource.Name == "Example.MyEventCounterSource")
        {
            var args = new Dictionary<string, string?> { ["EventCounterIntervalSec"] = "1" };
            EnableEvents(eventSource, EventLevel.Verbose, EventKeywords.All, args);
        }
    }

    protected override void OnEventWritten(EventWrittenEventArgs eventData)
    {
        if (eventData.EventName != "EventCounters" || eventData.Payload is null)
        {
            return;
        }

        if (eventData.Payload.First() is not IDictionary<string, object> payload)
        {
            return;
        }

        Console.WriteLine($"{payload["DisplayName"]} - {payload["Increment"]}");
    }
}