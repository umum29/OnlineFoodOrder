namespace OnlineOrderApi.Models
{
  public class Dish
  {
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    //navigation property; by default, IList is nullable, so it is "zero to many" relations
    //public List<Order> Orders { get; } = new();
  }
}