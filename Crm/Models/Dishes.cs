namespace Crm.Domain.Models;

public class Dishes
{
    public int Id { get; set; }
    public string? Title { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public DishCategories Category { get; set; }
    public List<DishComponents> DishComponents { get; set; }
    public string? Recipe { get; set; } = String.Empty;
    public Double PrimeCost { get; set; }
    public Double Cost { get; set; }
    public List<Checks> Checks { get; set; } = [];

}