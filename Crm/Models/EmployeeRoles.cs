namespace Crm.Domain.Models
{
    public class EmployeeRoles
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Employees> Employees { get; set; } = [];
    }
}
