namespace ClaimSystem.Models
{
    public class Invoice
    {
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public List<Claims> Claims { get; set; }
        public decimal TotalAmount { get; set; }
    }
}