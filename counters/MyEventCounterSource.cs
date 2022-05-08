using System.Diagnostics.Tracing;

namespace counters;

[EventSource(Name = "Example.MyEventCounterSource")]
public sealed class MyEventCounterSource : EventSource
{
    public static readonly MyEventCounterSource Instance = new();
    private readonly IncrementingEventCounter _myCounter;

    private MyEventCounterSource() =>
        _myCounter = new IncrementingEventCounter("my-counter", this)
        {
            DisplayName = "My Incrementing Counter"
        };

    public void Up() => _myCounter.Increment();

    protected override void Dispose(bool disposing)
    {
        _myCounter.Dispose();
        base.Dispose(disposing);
    }
}