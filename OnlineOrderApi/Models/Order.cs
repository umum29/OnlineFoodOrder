using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineOrderApi.Models
{
  //For Order table, it is dependent table, so we define foreign key in this table
  public class Order
  {
    public int Id { get; set; }

    public DateTime CreatedTime { get; set; }
    public DateTime PickupTime { get; set; }
    public decimal TotalPrice { get; set; }
    //navigation properties
    //[Required]//define "one to many" relation instead of "zero to many"
    public List<Dish> Dishes { get; } = new();

    //navigation property for Customer table
    [ForeignKey("Customer")]
    public int CustomerId { get; set; }
    //must use ? to prevent creating a new Customer record in Customer table
    public Customer? Customer { get; set; }

    //navigation property for Employee table
    [ForeignKey("Employee")]
    public int EmployeeId { get; set; }
    //must use ? to prevent creating a new Emplyee record in Employee table
    public Employee? Employee { get; set; }

  }
}