namespace Crm.Domain.Models
{
    public class Clients
    {
        public int Id { get; set; }
        public string Initials { get; set; } = string.Empty;
        public int LoyalityId { get; set; }
        public Loyalities Loality { get; set; }
        public List<Orders> Orders { get; set; } = [];
    }
}
