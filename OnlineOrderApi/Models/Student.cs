using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace OnlineOrderApi.Models
{
  /* Scenario 1: one to one relation
  public class Student
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int StudentId { get; set; }
    public string StudentName { get; set; }
  }
  public class Grade
  {
    public int GradeId { get; set; }
    public string GradeName { get; set; }
    public string Section { get; set; }

    [ForeignKey("Student")]
    public int StudentId { get; set; }
    public Student? Student { get; set; }
  }
*/
  /*
    //Scenario 2: one to many relation
    public class Student// Principal (parent)
    {
      public int StudentId { get; set; }
      public string StudentName { get; set; }

      //to prevent “A possible object cycle was detected”, 
      //please add "builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);" && "using System.Text.Json.Serialization;" in program.cs
      public ICollection<Grade> Grades { get; } = new List<Grade>();// Collection navigation containing dependents

    }
    public class Grade // Dependent (child)
    {
      public int GradeId { get; set; }
      public string GradeName { get; set; }
      public string Section { get; set; }

      public int StudentId { get; set; }  // Required foreign key property
      public Student? Student { get; set; }// Required reference navigation to principal
    }
    */
  //Scenario 3: many to many relation
  public class Student// Principal (parent)
  {
    public int Id { get; set; }
    public string StudentName { get; set; }

    //to prevent “A possible object cycle was detected”, 
    //please add "builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);" && "using System.Text.Json.Serialization;" in program.cs
    public List<StudentGrade> StudentGrade { get; set; } // Collection navigation containing dependents

  }
  public class Grade // Dependent (child)
  {
    public int Id { get; set; }
    public string GradeName { get; set; }
    public string Section { get; set; }
    public List<StudentGrade> StudentGrade { get; set; }

  }
  public class StudentGrade
  {
    public int StudentId { get; set; }
    public Student Student { get; set; }

    public int GradeId { get; set; }
    public Grade Grade { get; set; }
  }

}