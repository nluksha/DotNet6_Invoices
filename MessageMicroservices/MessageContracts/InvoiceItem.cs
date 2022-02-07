namespace MessageContracts
{
    public class InvoiceItem
    {
        public string Description { get; set; }
        public double Price { get; set; }
        public double ActualMileage { get; set; }
        public double BaseRate { get; set; }
        public bool IsOversized { get; set; }
        public bool IsRefrigerated { get; set; }
        public bool IsHazardousMaterial { get; set; }
    }
}