using System;
using Microsoft.EntityFrameworkCore;
using OnlineOrderApi.Models;

namespace OnlineOrderApi.Data
{
  public class ApplicationDataContext : DbContext
  {
    public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options) : base(options) { }

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Dish> Dishes => Set<Dish>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Grade> Grades => Set<Grade>();
    public DbSet<StudentGrade> StudentGrade { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      /*
      modelBuilder.Entity<Student>()
       .HasMany(e => e.Grades)
       .WithOne(e => e.Student)
       .HasForeignKey(e => e.StudentId)
       .IsRequired(false);
       */
      /*
            modelBuilder.Entity<Order>().HasMany(o => o.Dishes)
            .WithMany();
      */
      //modelBuilder.Entity<Student>().HasMany(o => o.Grades).WithMany(e => e.Students);
      //modelBuilder.Entity<Order>().HasOne(o => o.Customer).HasForeignKey(o => o.Customer);
      //base.OnModelCreating(modelBuilder);
      //for many-to-many relations
      modelBuilder.Entity<StudentGrade>().HasKey(sc => new { sc.StudentId, sc.GradeId });

      modelBuilder.Entity<StudentGrade>()
          .HasOne<Student>(sc => sc.Student)
          .WithMany(s => s.StudentGrade)
          .HasForeignKey(sc => sc.StudentId);

      modelBuilder.Entity<StudentGrade>()
          .HasOne<Grade>(sc => sc.Grade)
          .WithMany(s => s.StudentGrade)
          .HasForeignKey(sc => sc.GradeId);

    }

  }
}
