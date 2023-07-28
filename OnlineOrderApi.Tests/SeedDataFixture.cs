using OnlineOrderApi.Models;
using OnlineOrderApi.Data;
using Microsoft.EntityFrameworkCore;

namespace OnlineOrderApi.Tests
{
  public class SeedDataFixture : IDisposable
  {
    public ApplicationDataContext _context { get; private set; }

    public SeedDataFixture()
    {
      var dbContextOptions = new DbContextOptionsBuilder<ApplicationDataContext>()
        .UseInMemoryDatabase(databaseName: "OnlineOrder")
        .Options;
      _context = new ApplicationDataContext(dbContextOptions);
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
      _context.StudentGrade.AddRange(studentGrades);
      _context.SaveChanges();
    }
    public void Dispose()
    {
      _context.Dispose();
    }
  }
}