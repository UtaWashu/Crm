namespace Crm.Domain.Models
{
    public class Payments
    {
        public int Id { get; set; }
        public string Title { get; set; } = String.Empty;
        public List<Orders> Orders { get; set; } = [];

    }
}
