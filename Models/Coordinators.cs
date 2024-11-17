namespace ClaimSystem.Models
{
    public class Coordinators
    {
        public int CoordinatorId { get; set; }
        public string Name { get; set; }

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
