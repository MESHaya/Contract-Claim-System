using Microsoft.EntityFrameworkCore;

namespace ClaimSystem.Models
{
    public class ClaimsDbContext : DbContext
    {
        public DbSet<Claim> claims {  get; set; }


        public ClaimsDbContext(DbContextOptions<ClaimsDbContext> options)
       :base(options)
        {
            
        }
    }
}
