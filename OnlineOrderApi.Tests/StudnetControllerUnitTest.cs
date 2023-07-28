using OnlineOrderApi.Models;
using OnlineOrderApi.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using OnlineOrderApi.Data;
using OnlineOrderApi.Repository;
using OnlineOrderApi.Repository.Interface;
using AutoMapper;
using System.Net;
using Moq;


namespace OnlineOrderApi.Tests;
public class StudentControllerUnitTest : IClassFixture<SeedDataFixture>
{
  public StudentRepository _repository;
  public static IMapper _mapper;
  SeedDataFixture _fixture;

  //constructor2-->public
  public StudentControllerUnitTest(SeedDataFixture fixture)
  {
    _fixture = fixture;
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
    //setup IMapper/Automapper DI
    if (_repository == null)
    {
      _repository = new StudentRepository(_fixture._context);
    }
  }

  [Fact]
  public async void GetStudents_DefaultNoParameter_ReturnStudentList()
  {
    //Arrange
    var controller = new StudentController(_repository, _mapper);
    //Act
    var response = await controller.GetStudents();
    //must convert each object step by step
    var okResult = response.Result as ObjectResult;
    APIResponse apiResponse = okResult.Value as APIResponse;
    List<Dto.StudentDTO> studentList = apiResponse.Result as List<Dto.StudentDTO>;

    //Assert
    Assert.NotNull(apiResponse);
    Assert.Equal(2, studentList.Count);
    //Assert.IsType<OkObjectResult>(response);
  }
  [Fact]
  public async void GetStudent_IdAsParameter_ReturnSingleStudent()
  {
    //Arrange
    var mockRepository = new Mock<IStudentRepository>();
    int testStudentId = 2;
    Student mockResult = new Student() { Id = 2, StudentName = "Tom" };
    mockRepository.Setup(x => x.GetAsync(x => x.Id == testStudentId, true)).Returns(Task.FromResult(mockResult));
    /*
    mockRepository.Setup(x => x.GetById(42))
        .Returns(new Product { Id = 42 });
        */

    var controller = new StudentController(mockRepository.Object, _mapper);
    //Act
    var response = await controller.GetStudent(2);
    //must convert each object step by step
    var okResult = response.Result as ObjectResult;
    APIResponse apiResponse = okResult.Value as APIResponse;
    Dto.StudentDTO studentData = apiResponse.Result as Dto.StudentDTO;

    //Assert
    Assert.NotNull(apiResponse);
    Assert.Equal(2, studentData.Id);
    Assert.Equal("Tom", studentData.StudentName);
    //Assert.IsType<OkObjectResult>(response);
  }
}