using Microsoft.EntityFrameworkCore;
using OnlineOrderApi.Data;
using OnlineOrderApi.Models;

namespace OnlineOrderApi;

public class DataSeeder
{
  public static void Initialize(IServiceProvider serviceProvider)
  {
    using (var dbContext = new ApplicationDataContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDataContext>>()))
    {
      if (!dbContext.Employees.Any())
      {
        var employeeList = new List<Employee>()
        {
            new Employee()
            {
                Name = "David",
                Phone="111222333"
            },
            new Employee()
            {
              Name = "Alex",
              Phone="7145558888"
            }
        };
        dbContext.Employees.AddRange(employeeList);
        dbContext.SaveChanges();
      }

      if (!dbContext.Customers.Any() && !dbContext.Dishes.Any())
      {
        var customerList = new List<Customer>()
        {
          new Customer()
          {
            Name = "Kitty",
            Email = "kitty@hotmail.com",
            Phone = "2148886666"
          },
          new Customer()
          {
            Name = "Tom",
            Email = "Tom@hotmail.com",
            Phone = "342665555"
          }
        };
        dbContext.Customers.AddRange(customerList);

        var dishList = new List<Dish>()
        {
          new Dish()
          {
            Name = "beef noodle",
            Price = 15.75m
          },
          new Dish()
          {
            Name = "fried chicken bento",
            Price = 17.25m
          }
        };
        dbContext.Dishes.AddRange(dishList);
        dbContext.SaveChanges();
      }
      //For many-to-many, create basic "Student" & "Grade" & "join table(StudentGrade)" at the same time
      if (!dbContext.StudentGrade.Any())
      {
        var studentGrades = new List<StudentGrade>()
        {
          new StudentGrade()
          {
            Student = new Student()
            {
              StudentName = "Kai"
            },
            Grade = new Grade()
            {
              GradeName="GradeName1",
              Section="History"
            }
          },
          new StudentGrade()
          {
            Student = new Student()
            {
              StudentName = "Tom"
            },
            Grade = new Grade()
            {
              GradeName="GradeName2",
              Section="Math"
            }
          }
        };
        dbContext.StudentGrade.AddRange(studentGrades);
        dbContext.SaveChanges();
      }
    }
  }
}


