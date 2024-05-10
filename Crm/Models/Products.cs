namespace Crm.Domain.Models;

public class Products
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public ProductCategories Category { get; set; }
    public List<Invoices> Invoices { get; set; } = [];
    public List<Storages> Storages { get; set; } = [];
    public List<DishComponents> DishComponents { get; set; } = [];
}