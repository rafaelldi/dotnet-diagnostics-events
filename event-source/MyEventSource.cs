using System.Diagnostics.Tracing;

namespace event_source;

[EventSource(Name = "Example.MyEventSource")]
public sealed class MyEventSource : EventSource
{
    public static readonly MyEventSource Instance = new();

    [Event(1)]
    public void ValueReceived(int value) => WriteEvent(1, value);
}