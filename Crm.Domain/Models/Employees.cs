namespace Crm.Domain.Models
{
    public class Employees
    {
        public int Id { get; set; }
        public string Initials { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public EmployeeRoles Role { get; set; }
        //public int AccountId { get; set; }
        public Accounts Account { get; set; }

        public List<Orders> Orders { get; set; } = [];
    }
}
