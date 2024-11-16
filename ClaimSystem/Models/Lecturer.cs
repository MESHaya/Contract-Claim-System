using System.ComponentModel.DataAnnotations;

namespace ClaimSystem.Models
{
    public class Lecturer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactInfo { get; set; }
        public DateTime DateOfHire { get; set; }
    }

}
