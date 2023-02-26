using Microsoft.EntityFrameworkCore;
using Organizer.Models;

namespace Organizer.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<TodoItem> TodoItem { get; set; }
    }
}
