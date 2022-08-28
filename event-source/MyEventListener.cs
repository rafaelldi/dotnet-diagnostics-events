using System.Diagnostics.Tracing;

namespace event_source;

public sealed class MyEventListener : EventListener
{
    protected override void OnEventSourceCreated(EventSource eventSource)
    {
        if (eventSource.Name == "Example.MyEventSource")
        {
            EnableEvents(eventSource, EventLevel.Informational);
        }
    }

    protected override void OnEventWritten(EventWrittenEventArgs eventData)
    {
        if (eventData.EventName != nameof(MyEventSource.ValueReceived) || eventData.Payload is null)
        {
            return;
        }

        Console.WriteLine($"Event received: {eventData.Payload[0]}");
    }
}