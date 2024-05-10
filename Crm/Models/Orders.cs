namespace Crm.Domain.Models;

public class Orders
{
    public Guid Id { get; set; }
    public int EmployeeId { get; set; }
    public Employees Employee { get; set; }
    public int ClientId { get; set; }
    public Clients Client { get; set; }
    public double Total { get; set; }
    public int PeopleCount { get; set; }
    
    public int TableNumber { get; set; }
    public DateTime Date { get; set; } 
    public int PaymentId { get; set; }
    public Payments Payment { get; set; }
    public List<Checks> Checks { get; set; } = [];
    public int OrderStatusId { get; set; }
    public OrderStatus OrderStatus { get; set; }
}