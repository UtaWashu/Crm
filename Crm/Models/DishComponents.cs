namespace Crm.Domain.Models;

public class DishComponents
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Products Product { get; set; }
    public int DishId { get; set; }
    public Dishes Dish { get; set; }
    public int Quantity { get; set; }
}