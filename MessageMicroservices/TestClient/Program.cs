using GreenPipes;
using MassTransit;
using MessageContracts;
using TestClient;

const int RETRY_COUNT = 5;
const int RETRY_INTERVAL_SECONDS = 10;
const int CANCELLATION_DELAY_SECCONDS = 10;

Console.WriteLine("Waiting while consumers initialize.");
await Task.Delay(3000);

var busControll = Bus.Factory.CreateUsingRabbitMq(config =>
{
    config.Host("localhost");
    config.ReceiveEndpoint("invoice-service-created", e =>
    {
        e.UseInMemoryOutbox();
        e.Consumer<InvoiceCreatedConsumer>(c => c.UseMessageRetry(m => m.Interval(RETRY_COUNT, new TimeSpan(0, 0, RETRY_INTERVAL_SECONDS))));
    });
});
var source = new CancellationTokenSource(TimeSpan.FromSeconds(CANCELLATION_DELAY_SECCONDS));

await busControll.StartAsync(source.Token);

var keyCount = 0;

try
{
    Console.WriteLine("Enter any key to send an invoice request or Q to quit.");

    while (Console.ReadKey(true).Key != ConsoleKey.Q)
    {
        keyCount++;
        await SendRequestForInvoiceCreation(busControll);

        Console.WriteLine($"Enter any key to send an invoice request or Q to quit. {keyCount}");
    }
}
finally
{
    await busControll.StopAsync();
}

static async Task SendRequestForInvoiceCreation(IPublishEndpoint publishEndpoint)
{
    var rnd = new Random();
    await publishEndpoint.Publish<IInvoiceToCreate>(new
    {
        CustomerNumber = rnd.Next(1000, 9999),
        InvoiceItems = new List<InvoiceItem>
        {
            new InvoiceItem
            {
                Description = "Tables",
                Price = Math.Round(rnd.NextDouble() * 100, 2),
                ActualMileage = 40,
                BaseRate = 12.50,
                IsHazardousMaterial = false,
                IsOversized = true,
                IsRefrigerated = false
            },
            new InvoiceItem
            {
                Description = "Chairs",
                Price = Math.Round(rnd.NextDouble() * 100, 2),
                ActualMileage = 40,
                BaseRate = 4.75,
                IsHazardousMaterial = false,
                IsOversized = false,
                IsRefrigerated = false
            }
        }
    });
}