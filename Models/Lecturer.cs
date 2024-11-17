using System.ComponentModel.DataAnnotations;

namespace ClaimSystem.Models
{
    public class Lecturer
    {
        public int  LecturerId { get; set; }  // Foreign key to Lecturer

        public string Name { get; set; }
        public string ContactInfo { get; set; }
        public DateTime DateOfHire { get; set; }
        public string Username
        {
            get; set;
        }

        public string Password
        {
            get; set;
        }
    }
}


