namespace Crm.Domain.Models
{
    public class Providers
    {
        public int Id { get; set; }
        public string Initials { get; set; } = String.Empty;
        public List<Deliveries> Deliveries { get; set; } = [];

    }
}
