namespace Crm.Domain.Models;

public class Invoices
{
    public int Id { get; set; }
    public int DeliveryId { get; set; }
    public Deliveries Delivery { get; set; }
    public int ProductId { get; set; }
    public Products Product { get; set; }
    public int Quantity { get; set; }
}