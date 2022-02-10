using MassTransit;
using MessageContracts;

namespace PaymentMicroservice
{
    public class InvoiceCreatedConsumer : IConsumer<IInvoiceCreated>
    {
        public async Task Consume(ConsumeContext<IInvoiceCreated> context)
        {
            await Task.Run(() =>
            {
                Console.WriteLine($"Recieved message for invoices number: {context.Message.InvoiceNumber}");
            });
        }
    }
}
