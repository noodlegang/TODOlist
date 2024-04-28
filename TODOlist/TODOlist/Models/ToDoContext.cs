using Microsoft.EntityFrameworkCore;

namespace TODOlist.Models
{
    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options){ }
        public DbSet<ToDoItem> ToDos { get; set; } = null!;
        public DbSet<Status> Statuses { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Status>().HasData(
                new Status { StatusId = "Ready", StatusName = "Ready"},
                new Status { StatusId = "In Progress", StatusName = "In progress" },
                new Status { StatusId = "Done", StatusName = "Done" });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlite(@"DataSource=mydatabase.db;");
        }
    }
}
