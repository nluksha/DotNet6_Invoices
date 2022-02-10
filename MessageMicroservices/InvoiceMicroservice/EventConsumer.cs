using MassTransit;
using MessageContracts;

namespace InvoiceMicroservice
{
    public class EventConsumer : IConsumer<IInvoiceToCreate>
    {
        public async Task Consume(ConsumeContext<IInvoiceToCreate> context)
        {
            var newInvoiceMessage = new Random().Next(10000, 99999);
            var customer = context.Message.CustomerNumber;
            Console.WriteLine($"Creating invoice {newInvoiceMessage} for customer: {customer}");

            context.Message.InvoiceItems.ForEach(i =>
            {
                Console.WriteLine($"With items: Price: {i.Price}, Desc: {i.Description}");
                Console.WriteLine($"Actual distance i miles: {i.ActualMileage}, Base Rate: {i.BaseRate}");
                Console.WriteLine($"Oversized: {i.IsOversized}, Refrigerated: {i.IsRefrigerated}, HazMat: {i.IsHazardousMaterial}");
            });

            await context.Publish<IInvoiceCreated>(new
            {
                InvoiceNumber = newInvoiceMessage,
                InvoiceData = new
                {
                    context.Message.CustomerNumber,
                    context.Message.InvoiceItems
                }
            });
        }
    }
}
