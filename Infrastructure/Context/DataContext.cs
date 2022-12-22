using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
namespace Infrastructure.Context;

public class DataContext : IdentityDbContext<IdentityUser>
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>()
       .HasOne<Job>(s => s.Job)
       .WithMany(g => g.Employees)
       .HasForeignKey(s => s.JobId);

        modelBuilder.Entity<JobHistory>()
        .HasOne<Job>(s => s.Job)
        .WithMany(g => g.JobHistories)
        .HasForeignKey(s => s.JobId);


        modelBuilder.Entity<JobHistory>()
        .HasOne<Employee>(s => s.Employee)
        .WithMany(g => g.JobHistories)
        .HasForeignKey(s => s.EmployeeId);

        
 base.OnModelCreating(modelBuilder);
    }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<JobTimeHistory> JobTimeHistories { get; set; }
    public DbSet<JobHistory> JobHistories { get; set; }
    
}