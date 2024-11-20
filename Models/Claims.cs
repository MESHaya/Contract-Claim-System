using System.ComponentModel.DataAnnotations;

namespace ClaimSystem.Models
{
    public class Claims
    {
        public int Id { get; set; }  // Primary key for the claim
        public int StatusId { get; set; }

        public string LecturerName { get; set; }


        public decimal HoursWorked { get; set; }  // Hours worked
        public decimal HourlyRate { get; set; }  // Hourly rate of the lecturer
        public string? Notes { get; set; }  // Optional additional notes
        [Required]
        public string Status { get; set; }
        public string? FileName { get; set; }
      
        public DateTime DateSubmitted { get; set; }  // Date of submission

        public string? RejectionReason {  get; set; }

        public int LecturerId { get; set; }  // Foreign key to Lecturer



    }
}
