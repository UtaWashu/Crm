using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Crm.Domain;
using Crm.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Crm.Controllers
{
    public class HomeController : Controller
    {
        private readonly CrmDbContext _context;

        public HomeController(CrmDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Login(string login, string password)
        {
            var account = _context.Accounts.FirstOrDefault(a => a.Login == login && a.Password == password);

            if (account != null)
            {
                var employee = _context.Employees.Include(e => e.Role).FirstOrDefault(e => e.Id == account.Id);

                if (employee.Role.Title == "Admin")
                {
                    return RedirectToAction("Admin", "Admin");
                }
                else if (employee.Role.Title == "Waiter")
                {
                    return RedirectToAction("Index", "Waiter");
                }
            }

            return View("Error");
        }

    }
}