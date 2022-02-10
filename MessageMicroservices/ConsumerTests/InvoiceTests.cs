using FluentAssertions;
using MassTransit;
using MassTransit.Testing;
using MessageContracts;
using PaymentMicroservice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ConsumerTests
{
    public class InvoiceTests
    {
        [Fact]
        public async Task Verify_InvoiceCreatedMessage_Consummed()
        {
            var harness = new InMemoryTestHarness();

            var consumerHarness = harness.Consumer<InvoiceCreatedConsumer>();
            await harness.Start();

            try
            {
                await harness.Bus.Publish<IInvoiceCreated>(
                    new { InvoiceNumber = InVar.Id });

                Assert.True(await harness.Consumed.Any<IInvoiceCreated>());
                Assert.True(await consumerHarness.Consumed.Any<IInvoiceCreated>());

                harness.Published.Select<IInvoiceCreated>().Count().Should().Be(1);
            } finally
            {
                await harness.Stop();
            }
        }
    }
}
