using Microsoft.EntityFrameworkCore;
using TodoList.Data.Model;

namespace TodoList.Data
{
    public class TaskDbContext:DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options)
            : base(options)
        {
           
        }
        public DbSet<MyTask> Taskes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MyTask>(t =>
            {
                t.ToTable("Taskes");
                t.Property(t => t.Title).IsRequired();
                t.Property(t=>t.StartDate).IsRequired();
                t.Property(t => t.EndDate).HasDefaultValue();
            });
        }
    }
}
