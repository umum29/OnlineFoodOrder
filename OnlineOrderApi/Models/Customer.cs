namespace OnlineOrderApi.Models
{
  public class Customer
  {
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }

    //navigation property
    //public int OrderId { get; set; }
    //public List<Order> Orders { get; set; } = null

  }
}