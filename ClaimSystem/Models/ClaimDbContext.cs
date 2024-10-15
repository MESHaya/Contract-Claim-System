using ClaimSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ClaimSystem.Models
{
    public class ClaimDbContext : DbContext
    {
        public DbSet<Claims> Claim { get; set; }//database table 

        public ClaimDbContext(DbContextOptions<ClaimDbContext> options)
            : base(options)
        {

        }


    }
}
