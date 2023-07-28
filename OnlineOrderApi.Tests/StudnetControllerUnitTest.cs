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
    var okResult = response.Result as ObjectResult;
    APIResponse apiResponse = okResult.Value as APIResponse;
    List<Dto.StudentDTO> studentList = apiResponse.Result as List<Dto.StudentDTO>;

    //OkObjectResult okResult = (OkObjectResult)response.Result;
    //Assert
    Assert.NotNull(apiResponse);
    Assert.Equal(2, studentList.Count);
    //Assert
    //Assert.IsType<OkObjectResult>(response);
    //Assert.Equal(true, response.Result.Value.IsSuccess);
    //Assert.NotNull(response.Result);

  }
}