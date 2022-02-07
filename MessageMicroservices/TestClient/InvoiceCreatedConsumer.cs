using MassTransit;
using MessageContracts;

namespace TestClient
{
    internal class InvoiceCreatedConsumer : IConsumer<IInvoiceCreated>
    {
        public async Task Consume(ConsumeContext<IInvoiceCreated> context)
        {
            await Task.Run(() =>
            {
                Console.WriteLine($"Invoice with number: {context.Message.InvoiceNumber} was created.");
            });
        }
    }
}
