using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using OnlineOrderApi.Data;
using OnlineOrderApi.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using OnlineOrderApi.Repository;
using OnlineOrderApi.Repository.Interface;
using System.Linq;
using MockQueryable.Moq;

namespace OnlineOrderApi.Tests;

public class StudentRepositoryTests
{

  [Fact]
  public async void GetByIdAsync_FindStudentById_ReturnStudent()
  {
    //for testing dbContext's async method, use MockQueryable.Moq
    var mockData = new List<Student>() { new Student() { Id = 1, StudentName = "kai" }, new Student() { Id = 2, StudentName = "tom" } }.AsQueryable().BuildMock().BuildMockDbSet();

    var options = new DbContextOptionsBuilder<ApplicationDataContext>()
                   .UseInMemoryDatabase(Guid.NewGuid().ToString())
                   .Options;
    var mockContext = new Mock<ApplicationDataContext>(options);
    mockContext.Setup(c => c.Set<Student>()).Returns(mockData.Object);

    //Execute method of SUT (ProductsRepository)  
    var studentRepository = new StudentRepository(mockContext.Object);
    var student = await studentRepository.GetAsync(x => x.Id == 2, false);

    //Assert  
    Assert.NotNull(student);
    //to verify whether "student" data is "Student" type
    Assert.IsAssignableFrom<Student>(student);
    Assert.True(student.Id == 2);
  }

  [Fact]
  public async void GetAllAsync_NoParameter_ReturnAllStudents()
  {
    //for testing dbContext's async method, use MockQueryable.Moq
    var mockData = new List<Student>() { new Student() { Id = 1, StudentName = "kai" }, new Student() { Id = 2, StudentName = "tom" } }.AsQueryable().BuildMock().BuildMockDbSet();

    var options = new DbContextOptionsBuilder<ApplicationDataContext>()
                   .UseInMemoryDatabase(Guid.NewGuid().ToString())
                   .Options;
    var mockContext = new Mock<ApplicationDataContext>(options);
    mockContext.Setup(c => c.Set<Student>()).Returns(mockData.Object);

    //Execute method of SUT (ProductsRepository)  
    var studentRepository = new StudentRepository(mockContext.Object);
    var studentList = await studentRepository.GetAllAsync();

    //Assert  
    Assert.NotNull(studentList);
    //to verify whether "student" data is "Student" type
    Assert.IsAssignableFrom<List<Student>>(studentList);
    Assert.True(studentList.Count == 2);
  }

  [Fact]
  public async void CreateAsync_StudentAsParameter_Return()
  {
    //for testing dbContext's async method, use MockQueryable.Moq
    var studentEntities = new List<Student>() { new Student() { Id = 1, StudentName = "kai" }, new Student() { Id = 2, StudentName = "tom" } };
    var mockDbset = studentEntities.AsQueryable().BuildMockDbSet();
    //instruct how mockDbset react to AddAsync method=> actually use studentEntities List to do "Add" action in the list
    mockDbset.Setup(set => set.AddAsync(It.IsAny<Student>(), It.IsAny<CancellationToken>()))
          .Callback((Student entity, CancellationToken _) => studentEntities.Add(entity)); ;

    var options = new DbContextOptionsBuilder<ApplicationDataContext>()
                   .UseInMemoryDatabase(Guid.NewGuid().ToString())
                   .Options;
    var mockContext = new Mock<ApplicationDataContext>(options);
    mockContext.Setup(c => c.Set<Student>()).Returns(mockDbset.Object);

    //Execute method of SUT (ProductsRepository)  
    var studentRepository = new StudentRepository(mockContext.Object);
    var newStudent = new Student() { Id = 3, StudentName = "Cage" };
    await studentRepository.CreateAsync(newStudent);

    //Assert  
    var entity = mockDbset.Object.FirstOrDefaultAsync(x => x.Id == 3).Result;
    Assert.NotNull(entity);
    //to verify whether "student" data is "Student" type
    Assert.True(entity.Id == 3);
    Assert.IsAssignableFrom<Student>(entity);
  }
  [Fact]
  public async void DeleteAsync_StudentAsParameter_Return()
  {
    //for testing dbContext's async method, use MockQueryable.Moq
    var studentEntities = new List<Student>() { new Student() { Id = 1, StudentName = "kai" }, new Student() { Id = 2, StudentName = "tom" } };
    var mockDbset = studentEntities.AsQueryable().BuildMockDbSet();
    //instruct how mockDbset react to AddAsync method=> actually use studentEntities List to do "Add" action in the list

    mockDbset.Setup(set => set.Remove(It.IsAny<Student>()))
          .Callback((Student student) => studentEntities.RemoveAll(a => a.Id == student.Id)); ;

    var options = new DbContextOptionsBuilder<ApplicationDataContext>()
                   .UseInMemoryDatabase(Guid.NewGuid().ToString())
                   .Options;
    var mockContext = new Mock<ApplicationDataContext>(options);
    mockContext.Setup(c => c.Set<Student>()).Returns(mockDbset.Object);

    //Execute method of SUT (ProductsRepository)  
    var studentRepository = new StudentRepository(mockContext.Object);
    var removeStudent = new Student() { Id = 2, StudentName = "tom" };
    await studentRepository.RemoveAsync(removeStudent);

    //Assert  
    var entity = mockDbset.Object.FirstOrDefaultAsync(x => x.Id == 2).Result;
    Assert.Null(entity);
  }
  [Fact]
  public async void UpdateAsync_StudentAsParameter_Return()
  {
    //for testing dbContext's async method, use MockQueryable.Moq
    var studentEntities = new List<Student>() { new Student() { Id = 1, StudentName = "kai" }, new Student() { Id = 2, StudentName = "tom" } };
    var mockDbset = studentEntities.AsQueryable().BuildMockDbSet();
    //instruct how mockDbset react to AddAsync method=> actually use studentEntities List to do "Add" action in the list

    mockDbset.Setup(set => set.Update(It.IsAny<Student>()))
          .Callback((Student student) => { studentEntities.RemoveAll(a => a.Id == student.Id); studentEntities.Add(student); });

    var options = new DbContextOptionsBuilder<ApplicationDataContext>()
                   .UseInMemoryDatabase(Guid.NewGuid().ToString())
                   .Options;
    var mockContext = new Mock<ApplicationDataContext>(options);
    mockContext.Setup(c => c.Set<Student>()).Returns(mockDbset.Object);

    //Execute method of SUT (ProductsRepository)  
    var studentRepository = new StudentRepository(mockContext.Object);
    var updateStudent = new Student() { Id = 2, StudentName = "ChangedValue" };
    await studentRepository.UpdateAsync(updateStudent);

    //Assert  
    var entity = mockDbset.Object.FirstOrDefaultAsync(x => x.Id == 2).Result;
    Assert.NotNull(entity);
    Assert.IsAssignableFrom<Student>(entity);
    Assert.True(entity.StudentName == "ChangedValue");
  }

}
