using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineOrderApi.Models;
using OnlineOrderApi.Repository.Interface;
using OnlineOrderApi.Dto;
using AutoMapper;
using System.Net;
using Microsoft.AspNetCore.JsonPatch;
using System.Text.Json;
using System.Linq.Expressions;

namespace OnlineOrderApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class StudentController : ControllerBase
  {
    private readonly IStudentRepository _studentRepository;
    private readonly IMapper _mapper;
    private readonly APIResponse _response;

    public StudentController(IStudentRepository studentRepository, IMapper mapper)
    {
      _studentRepository = studentRepository;
      _mapper = mapper;
      _response = new();
    }

    // GET: api/Student
    [HttpGet]
    //ProducesResponseType is for SwaggerUI better demostrating Data schema in API
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<APIResponse>> GetStudents()
    {
      try
      {
        var studentList = await _studentRepository.GetAllAsync();
        _response.Result = _mapper.Map<List<StudentDTO>>(studentList);
        _response.StatusCode = HttpStatusCode.OK;
        return Ok(_response);
      }
      catch (Exception ex)
      {
        //be default, isSuccess = true;
        _response.IsSuccess = false;
        _response.ErrorMessages = new List<string>() { ex.ToString() };
      }
      return _response;
    }

    // GET: api/Student/5
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> GetStudent(int id)
    {
      try
      {
        if (id == 0)
        {
          _response.StatusCode = HttpStatusCode.BadRequest;
          return BadRequest(_response);
        }
        //repository only accepts Expression as parameter
        var student = await _studentRepository.GetAsync(s => s.Id == id);
        if (student == null)
        {
          _response.StatusCode = HttpStatusCode.NotFound;
          return NotFound(_response);
        }
        _response.Result = _mapper.Map<StudentDTO>(student);
        _response.StatusCode = HttpStatusCode.OK;
        return Ok(_response);
      }
      catch (Exception ex)
      {
        _response.IsSuccess = false;
        _response.ErrorMessages = new List<string>() { ex.ToString() };
      }
      return _response;
    }

    // PUT: api/Student/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> PutStudent(int id, [FromBody] StudentUpdateDTO updateDTO)
    {
      try
      {
        if (updateDTO == null || id != updateDTO.Id || await _studentRepository.GetAsync(x => x.Id == id, false) == null)
        {
          return BadRequest();
        }
        Student model = _mapper.Map<Student>(updateDTO);
        await _studentRepository.UpdateAsync(model);
        _response.IsSuccess = true;
        _response.StatusCode = HttpStatusCode.NoContent;

        return Ok(_response);
      }
      catch (Exception ex)
      {
        _response.IsSuccess = false;
        _response.ErrorMessages = new List<string>() { ex.ToString() };
      }

      return _response;
    }

    // POST: api/Student
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost("{gradeId}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> PostStudent([FromBody] StudentCreateDTO createDTO, int gradeId)
    {
      try
      {
        //define filter here for "Where" condition
        Expression<Func<Student, bool>> predicate = x => x.StudentName.ToLower() == createDTO.StudentName.ToLower();
        if (await _studentRepository.GetAsync(predicate, false) != null)
        //if (await _studentRepository.StudentExists(predicate))
        {
          ModelState.AddModelError("ErrorMessages", "Student already Exists!");
          return BadRequest(ModelState);
        }

        if (createDTO == null)
        {
          return BadRequest(createDTO);
        }
        //StudentCreateDTO doesn't have Id; StudentDTO has Id property
        Student student = _mapper.Map<Student>(createDTO);
        await _studentRepository.CreateManyToManyAsync(gradeId, student);
        _response.Result = _mapper.Map<StudentDTO>(student);
        _response.StatusCode = HttpStatusCode.Created;//201
        return Ok(_response);
      }
      catch (Exception ex)
      {
        _response.IsSuccess = false;
        _response.ErrorMessages
             = new List<string>() { ex.ToString() };
      }
      return _response;
    }

    // DELETE: api/Student/5
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<APIResponse>> DeleteStudent(int id)
    {
      try
      {
        if (id == 0)
        {
          return BadRequest();
        }
        var student = await _studentRepository.GetAsync(u => u.Id == id);
        if (student == null)
        {
          return NotFound();
        }
        await _studentRepository.RemoveManyToManyAsync(student);
        _response.StatusCode = HttpStatusCode.NoContent;
        _response.IsSuccess = true;
        return Ok(_response);
      }
      catch (Exception ex)
      {
        _response.IsSuccess = false;
        _response.ErrorMessages
             = new List<string>() { ex.ToString() };
      }
      return _response;
    }
    [HttpPatch("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    //patchDTO only contains partial propertyies needed to be updated
    //we need to retrieve orignal Model data from DB, then use .ApplyTo to map&create a whole Model class
    //finally, use this Model class to upate back to DB
    /*
    Sample Json request document:
    [
      {
        "path": "/StudentName",
        "op": "replace",
        "value": "testCahngedValue"
      }
    ]
    */
    public async Task<IActionResult> UpdatePartialStudent(int id, JsonPatchDocument<StudentUpdateDTO> patchDTO)
    {
      if (patchDTO == null || id == 0)
      {
        return BadRequest();
      }
      //NoTracking for incoming change
      var student = await _studentRepository.GetAsync(u => u.Id == id, false);
      if (student == null)
      {
        return BadRequest();
      }
      //old existing data in DB
      StudentUpdateDTO studentDTO = _mapper.Map<StudentUpdateDTO>(student);
      //important!!! use patchDTO to change the properties value of studentDTO
      //we need to install "dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson" for ModelState 
      //studentDTO will be updated
      patchDTO.ApplyTo(studentDTO, ModelState);
      //finally, convert the updated studentDTO(with changed values from patchDTO) to DB schema Moel class
      Student model = _mapper.Map<Student>(studentDTO);

      await _studentRepository.UpdateAsync(model);

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      return NoContent();
    }

  }
}
