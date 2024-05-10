
namespace Crm.Domain.Models
{
    public class Storages
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Products Product { get; set; }
        public Double Price { get; set; }
        public Double Remainder { get; set; }
    }
}
