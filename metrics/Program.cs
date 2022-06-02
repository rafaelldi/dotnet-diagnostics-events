using System.Diagnostics.Metrics;
using OpenTelemetry;
using OpenTelemetry.Metrics;

Console.WriteLine("Metrics sample application");
Console.WriteLine($"Current process id: {Environment.ProcessId}");

var meter = new Meter("Example.MyMeter");
var counter = meter.CreateCounter<int>("my-counter");

using var meterProvider = Sdk.CreateMeterProviderBuilder()
    .AddRuntimeMetrics()
    .AddMeter("Example.MyMeter")
    .Build();

using var myMeterListener = new MeterListener();
myMeterListener.InstrumentPublished = (instrument, listener) =>
{
    if (instrument.Meter.Name == "Example.MyMeter")
    {
        listener.EnableMeasurementEvents(instrument);
    }
};
myMeterListener.SetMeasurementEventCallback<int>(OnMeasurementWritten);
myMeterListener.Start();

Task.Run(async () => await StartMetricProducingTask());

Console.ReadKey();

async Task StartMetricProducingTask()
{
    while (true)
    {
        counter.Add(1);
        await Task.Delay(200);
    }
}

void OnMeasurementWritten(Instrument instrument, int measurement, ReadOnlySpan<KeyValuePair<string, object?>> tags,
    object? state)
{
    Console.WriteLine($"{instrument.Name} - {measurement}");
}
