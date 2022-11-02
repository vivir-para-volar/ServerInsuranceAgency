using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class DBContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<InsuranceEvent> InsuranceEvents { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<Policyholder> Policyholders { get; set; }
        public DbSet<PersonAllowedToDrive> PersonAllowedToDrives { get; set; }

        public DBContext(DbContextOptions<DBContext> options) : base(options)
        { 
        }
    }
}