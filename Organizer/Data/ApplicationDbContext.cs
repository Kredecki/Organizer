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
        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>()
                .HasOne(ti => ti.Project)
                .WithMany(p => p.TodoItems)
                .HasForeignKey(ti => ti.ProjectId);
        }
    }
}
