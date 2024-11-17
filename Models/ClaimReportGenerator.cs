/*using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data; // For DataTable
using System.IO; // For MemoryStream

public class ClaimReportGenerator
{
    public byte[] Generate(List<Claim> approvedClaims)
    {
        try
        {
            // Create and populate a DataTable
            DataTable claimsTable = new DataTable();
            claimsTable.Columns.Add("Id", typeof(int));
            claimsTable.Columns.Add("EmployeeName", typeof(string));
            claimsTable.Columns.Add("Amount", typeof(decimal));
            claimsTable.Columns.Add("Date", typeof(DateTime));

            foreach (var claim in approvedClaims)
            {
                claimsTable.Rows.Add(claim.Id, claim.EmployeeName, claim.Amount, claim.Date);
            }

            // Load the Crystal Report
            using ReportDocument reportDocument = new ReportDocument();
            string reportPath = "PathToYourReport.rpt"; // Update with the actual path
            reportDocument.Load(reportPath);
            reportDocument.SetDataSource(claimsTable);

            // Export the report to a PDF format
            using var stream = new MemoryStream();
            reportDocument.ExportToStream(ExportFormatType.PortableDocFormat).CopyTo(stream);
            return stream.ToArray();
        }
        catch (Exception ex)
        {
            // Log or handle the exception appropriately
            throw new Exception("An error occurred while generating the report: " + ex.Message, ex);
        }
    }
}*/
