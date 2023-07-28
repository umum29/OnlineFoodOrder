using OnlineOrderApi.Models;
using System.Linq.Expressions;

namespace OnlineOrderApi.Repository.Interface
{
  //for DI purpose
  //you can also declare some specific method here and implement it in StudentRepository
  public interface IStudentRepository : IRepository<Student>
  {
    Task CreateManyToManyAsync(int gradeId, Student student);
    Task RemoveManyToManyAsync(Student student);
  }
}