using Microsoft.EntityFrameworkCore;
using TodoList.Data.Model;

namespace TodoList.Data
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options)
            : base(options)
        {

        }
        public DbSet<MyTask> Taskes { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MyTask>(t =>
            {
                t.ToTable("Taskes");
                t.Property(t => t.Title).IsRequired();
                t.Property(t => t.StartDate).IsRequired();
                t.Property(t => t.EndDate);
                t.HasOne(t=>t.User)
                .WithMany(u=>u.Tasks)
                .HasForeignKey(t=>t.USerId);

            });

            modelBuilder.Entity<User>(u =>
            {
                u.ToTable("Users");
                u.Property(u => u.Name).HasMaxLength(128);
                u.Property(u => u.Name).IsRequired();
                u.Property(u => u.USerToken).HasMaxLength(16);
                u.HasIndex(u => u.USerToken).IsUnique();
                u.HasMany(u => u.Tasks)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.USerId);
            });


        }
    }
}
