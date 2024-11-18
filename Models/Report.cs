

namespace ClaimSystem.Models
{
    public class Report
    {
        public DateTime ReportDate { get; set; }
        public IEnumerable<Claims> Claims { get; set; }  // Ensure this matches the type of approvedClaims
        public decimal TotalClaimsAmount { get; set; }
        public string Summary { get; set; }
    }
}

