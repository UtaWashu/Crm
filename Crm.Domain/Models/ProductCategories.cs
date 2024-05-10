
namespace Crm.Domain.Models
{
    public class ProductCategories
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public List<Products> Products { get; set; } = [];
    }
}
