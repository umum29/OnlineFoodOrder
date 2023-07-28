namespace OnlineOrderApi.Dto
{
  //DTO class is prefered to deal with WebApi parameters; Model class is prefered to deal with DB schema
  //so. in most case, DTO fields is less than Model class's fields
  public class StudentDTO
  {
    public int Id { get; set; }
    public string StudentName { get; set; }
  }
  //only CreateDTO does not need Id property
  public class StudentCreateDTO
  {
    public string StudentName { get; set; }
  }
  public class StudentUpdateDTO
  {
    public int Id { get; set; }
    public string StudentName { get; set; }
  }
}