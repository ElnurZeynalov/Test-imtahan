using Microsoft.EntityFrameworkCore;
using TestImtahan.Models;

namespace TestImtahan.DAL
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions options):base(options)
        {
        }
        public DbSet<ClientsComments> Comments { get; set; }
    }
}
