using GreenPipes;
using InvoiceMicroservice;
using MassTransit;
using MessageContracts;

const int RETRY_COUNT = 5;
const int RETRY_INTERVAL_SECONDS = 10;
const int CANCELLATION_DELAY_SECCONDS = 10;
const int LISTENING_DELAY_MILLISECONDS = 100;

var busControll = Bus.Factory.CreateUsingRabbitMq(config =>
{
    config.Host("localhost");
    config.ReceiveEndpoint("invoice-service", e =>
    {
        e.UseInMemoryOutbox();
        e.Consumer<EventConsumer>(c => c.UseMessageRetry(m => m.Interval(RETRY_COUNT, new TimeSpan(0, 0, RETRY_INTERVAL_SECONDS))));
    });
});
var source = new CancellationTokenSource(TimeSpan.FromSeconds(CANCELLATION_DELAY_SECCONDS));

await busControll.StartAsync(source.Token);
Console.WriteLine("Invoice Microservice Now Listening");

try
{
    while (true)
    {
        await Task.Delay(LISTENING_DELAY_MILLISECONDS);
    }
}
finally
{
    await busControll.StopAsync();
}
