using Microsoft.EntityFrameworkCore;
using UserAuthenticationSystemV2.Models;

namespace UserAuthenticationSystemV2.Context
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
           
        }
    }
}