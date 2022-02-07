namespace MessageContracts
{
    public interface IInvoiceToCreate
    {
        int CustomerNumber { get; set; }
        List<InvoiceItem> InvoiceItems { get; set; }
    }
}