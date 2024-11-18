using System.ComponentModel.DataAnnotations;

namespace ClaimSystem.Models
{
    public class Lecturer
    {
        public int  LecturerId { get; set; }  // Foreign key to Lecturer

        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
       
        public string Department { get; set; }

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


