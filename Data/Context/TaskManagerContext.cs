using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class TaskManagerContext : DbContext
    {
        public TaskManagerContext(DbContextOptions<TaskManagerContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<ToDoTask> Task { get; set; }
        public DbSet<Announcement> Announcements { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoTask>().HasOne(t => t.CreaterUser).WithMany(u => u.CreatedTasks).HasForeignKey(u => u.CreaterUserId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ToDoTask>().HasOne(t => t.AsaignedUser).WithMany(u => u.Tasks).HasForeignKey(u => u.AsaignedUserId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ToDoTask>().HasOne(t => t.Department).WithMany(d => d.Tasks).HasForeignKey(t => t.DepartmentId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>().HasOne(u => u.Department).WithMany(d => d.Users).HasForeignKey(u => u.DepartmentId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
