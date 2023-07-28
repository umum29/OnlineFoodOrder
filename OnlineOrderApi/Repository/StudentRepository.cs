using OnlineOrderApi.Models;
using OnlineOrderApi.Data;
using OnlineOrderApi.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace OnlineOrderApi.Repository
{
  public class StudentRepository : Repository<Student>, IStudentRepository//for Dependency injection purpose
  {
    private readonly ApplicationDataContext _context;
    public StudentRepository(ApplicationDataContext context) : base(context)
    {
      _context = context;
    }

    //need to manually differentiate whether gradeId exists or not
    //Remember, when using StudentGrade table(join table), it will create "Student" & "Grade" & "StudentGrade" tables at the same tiem
    //but if "Grade" or "Student" is null, it will throw exception 
    /*
    public async Task<bool> StudentExists(Expression<Func<Student, bool>> filter)
    {
      //return await _context.Students.AnyAsync(filter);
      return await _context.Students.Where(filter).FirstOrDefaultAsync() != null;
    }
*/
    public async Task CreateManyToManyAsync(int gradeId, Student student)
    {
      var gradeEntity = await _context.Grades.Where(x => x.Id == gradeId).FirstOrDefaultAsync();
      if (gradeEntity != null)
      {
        var studentGrade = new StudentGrade() { Grade = gradeEntity, Student = student };
        await _context.AddAsync(studentGrade);
      }
      else
      {
        await _context.AddAsync(student);
      }
      await SaveAsync();
    }

    public async Task RemoveManyToManyAsync(Student student)
    {
      if (student != null)
      {
        var studentGradeList = await _context.StudentGrade.Where(x => x.StudentId == student.Id).ToListAsync();
        //there is no RemoveAsync
        if (studentGradeList != null && studentGradeList.Count > 0)
        {
          _context.RemoveRange(studentGradeList);
        }
        _context.Remove(student);
        await SaveAsync();
      }
    }
  }
}