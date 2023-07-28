using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineOrderApi.Data;
using OnlineOrderApi.Models;

namespace OnlineOrderApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class GradeController : ControllerBase
  {
    private readonly ApplicationDataContext _context;

    public GradeController(ApplicationDataContext context)
    {
      _context = context;
    }

    // GET: api/Grade
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Grade>>> GetGrades()
    {
      if (_context.Grades == null)
      {
        return NotFound();
      }
      //return await _context.Grades.Include(s => s.Student).ToListAsync();
      return await _context.Grades.ToListAsync();
    }

    // GET: api/Grade/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Grade>> GetGrade(int id)
    {
      if (_context.Grades == null)
      {
        return NotFound();
      }
      var grade = await _context.Grades.FindAsync(id);//await _context.Grades.Include(s => s.Student).FirstOrDefaultAsync(x => x.GradeId == id);

      if (grade == null)
      {
        return NotFound();
      }

      return grade;
    }

    // PUT: api/Grade/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutGrade(int id, Grade grade)
    {
      if (id != grade.Id)
      {
        return BadRequest();
      }

      _context.Entry(grade).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!GradeExists(id))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }

      return NoContent();
    }

    // POST: api/Grade
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Grade>> PostGrade(Grade grade, int StudentId)
    {
      if (_context.Grades == null)
      {
        return Problem("Entity set 'ApplicationDataContext.Grades'  is null.");
      }
      //for many-to-many relation, we must fullfill join table
      var studentEntity = await _context.Students.Where(x => x.Id == StudentId).FirstOrDefaultAsync();
      var gradeEntity = grade;
      var studentGrade = new StudentGrade()
      {
        Grade = gradeEntity,
        Student = studentEntity
      };
      _context.Add(studentGrade);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetGrade", new { id = grade.Id }, grade);
    }

    // DELETE: api/Grade/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGrade(int id)
    {
      if (_context.Grades == null)
      {
        return NotFound();
      }
      var grade = await _context.Grades.FindAsync(id);
      if (grade == null)
      {
        return NotFound();
      }

      _context.Grades.Remove(grade);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool GradeExists(int id)
    {
      return (_context.Grades?.Any(e => e.Id == id)).GetValueOrDefault();
    }
  }
}
