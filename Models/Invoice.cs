using System.Security.Claims;

public class Invoice
{
    public int InvoiceID { get; set; }
    public int LecturerID { get; set; }
    public DateTime GeneratedDate { get; set; }
    public List<Claim> Claims { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } // e.g., "Pending", "Paid"
}
