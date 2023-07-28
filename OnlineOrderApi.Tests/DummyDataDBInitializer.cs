using OnlineOrderApi.Models;

namespace OnlineOrderApi.Tests
{
  using OnlineOrderApi.Models;
  using OnlineOrderApi.Data;

  public class DummyDataDBInitializer
  {
    public void Seed(ApplicationDataContext context)
    {
      context.Database.EnsureDeleted();
      context.Database.EnsureCreated();

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
      context.SaveChanges();
    }
  }
}