using FluentAssertions;
using MassTransit;
using MassTransit.Testing;
using MessageContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using InvoiceMicroservice;

namespace ProducerTests
{
    public class InvoiceToCreateTests
    {
        [Fact]
        public async Task Verify_InvoiceToCreateCommant_Consumed()
        {
            var harness = new InMemoryTestHarness();
            var consumerHarness = harness.Consumer<EventConsumer>();

            await harness.Start();

            try
            {
                await harness.InputQueueSendEndpoint.Send<IInvoiceToCreate>(
                    new
                    {
                        CustomerNumber = 19282,
                        InvoiceItems = new List<InvoiceItem>
                        {
                            new InvoiceItem
                            {
                                Description = "Tables",
                                Price = Math.Round(1020.99),
                                ActualMileage = 40,
                                BaseRate = 12.50,
                                IsHazardousMaterial = false,
                                IsOversized = true,
                                IsRefrigerated = false
                            }
                        }
                    });

                Assert.True(await harness.Consumed.Any<IInvoiceToCreate>());
                Assert.True(await consumerHarness.Consumed.Any<IInvoiceToCreate>());

                harness.Published.Select<IInvoiceCreated>().Count().Should().Be(1);
            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}
