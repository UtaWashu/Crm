namespace Crm.Domain.Models;

public class Deliveries
{
    public int Id { get; set; }
    public int ProviderId { get; set; }
    public Providers Provider { get; set; }
    public DateTime Date { get; set; }
    public List<Invoices> Invoices { get; set; } = [];
}