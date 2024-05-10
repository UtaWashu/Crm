namespace Crm.Domain.Models
{
    public class Accounts
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        //public int EmployeeId { get; set; }
        public Employees Employee { get; set; }
    }
}
