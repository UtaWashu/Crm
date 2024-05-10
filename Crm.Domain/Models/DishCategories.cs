
namespace Crm.Domain.Models
{
    public class DishCategories
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public List<Dishes> Dishes { get; set; } = [];

    }
}
