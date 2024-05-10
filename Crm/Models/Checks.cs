namespace Crm.Domain.Models;

public class Checks
{
    public int Id { get; set; }
    public Guid OrderId { get; set; }
    public Orders Order { get; set;}
    public int DishId { get; set; }
    public Dishes Dish { get; set; }
    public int Quantity { get; set; }

}