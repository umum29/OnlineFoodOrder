using OnlineOrderApi.Models;
using OnlineOrderApi.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using OnlineOrderApi.Data;
using OnlineOrderApi.Repository;
using OnlineOrderApi.Repository.Interface;
using AutoMapper;
using System.Net;
using Microsoft.EntityFrameworkCore.InMemory;//dotnet add package Microsoft.EntityFrameworkCore.InMemory


namespace OnlineOrderApi.Tests;
public class StudentControllerUnitTest
{
  public StudentRepository _repository;
  public static DbContextOptions<ApplicationDataContext> dbContextOptions { get; }
  public static IMapper _mapper;

  //constructor2-->public
  public StudentControllerUnitTest()
  {

    //Arrange
    //1.Set up In-Memory DB
    var dbContextOptions = new DbContextOptionsBuilder<ApplicationDataContext>()
        .UseInMemoryDatabase(databaseName: "OnlineOrder")
        .Options;
    // Insert seed data into the database using one instance of the context
    using (var context = new ApplicationDataContext(dbContextOptions))
    {
      //DummyDataDBInitializer db = new DummyDataDBInitializer();
      //db.Seed(context);
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
      context.StudentGrade.AddRange(studentGrades);
      context.SaveChanges();
    }

    //setup IMapper/Automapper DI
    if (_mapper == null)
    {
      var mappingConfig = new MapperConfiguration(mc =>
      {
        mc.AddProfile(new MappingConfig());
      });
      IMapper mapper = mappingConfig.CreateMapper();
      _mapper = mapper;
    }

  }

  [Fact]
  public async void GetStudents_DefaultNoParameter_ReturnStudentList()
  {
    //Arrange
    //1.Set up In-Memory DB
    var dbContextOptions = new DbContextOptionsBuilder<ApplicationDataContext>()
        .UseInMemoryDatabase(databaseName: "OnlineOrder")
        .Options;
    // Insert seed data into the database using one instance of the context
    using (var context = new ApplicationDataContext(dbContextOptions))
    {
      //DummyDataDBInitializer db = new DummyDataDBInitializer();
      //db.Seed(context);
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
      context.StudentGrade.AddRange(studentGrades);
      context.SaveChanges();
      //setup IMapper/Automapper DI
      if (_repository == null)
      {
        _repository = new StudentRepository(context);
      }
      //Arrange
      var controller = new StudentController(_repository, _mapper);
      //Act
      var response = await controller.GetStudents();
      var okResult = response.Result as ObjectResult;
      APIResponse apiResponse = okResult.Value as APIResponse;
      List<Dto.StudentDTO> studentList = apiResponse.Result as List<Dto.StudentDTO>;

      //OkObjectResult okResult = (OkObjectResult)response.Result;
      //Assert
      Assert.NotNull(apiResponse);
      Assert.Equal(4, studentList.Count);
      //Assert
      //Assert.IsType<OkObjectResult>(response);
      //Assert.Equal(true, response.Result.Value.IsSuccess);
      //Assert.NotNull(response.Result);
    }
  }
}