namespace Crm.Domain.Models;

public class OrderStatus
{
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public List<Orders> Orders { get; set; } = [];
}