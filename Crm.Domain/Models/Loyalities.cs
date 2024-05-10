namespace Crm.Domain.Models
{
    public class Loyalities
    {
        public int Id { get; set; }
        public int Amount { get; set; } = 0;

        public List<Clients> Clients { get; set; } = [];
    }
}
