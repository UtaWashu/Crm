using Crm.Domain.Models;
using Crm.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crm.Controllers
{
    public class AdminController : Controller
    {
        private readonly CrmDbContext _context;

        public AdminController(CrmDbContext context)
        {
            _context = context;
        }
        public IActionResult Admin()
        {
            return View();
        }
        public IActionResult Invoice()
        {
            var providers = _context.Providers.ToList();
            ViewBag.Providers = providers;

            var groupedInvoices = _context.Invoices
                .Include(i => i.Delivery)
                .ThenInclude(d => d.Provider)
                .Include(i => i.Product)
                .ToList()
                .GroupBy(i => new { i.Delivery.Provider.Initials, i.Delivery.Date });

            return View(groupedInvoices);
        }

        [HttpPost]
        public async Task<IActionResult> AddMultipleData(string providerInitials, DateTime deliveryDate, List<string> productTitles, List<int> quantities)
        {
            var provider = await _context.Providers.FirstOrDefaultAsync(p => p.Initials == providerInitials);
            if (provider == null)
            {
                provider = new Providers { Initials = providerInitials };
                _context.Providers.Add(provider);
                await _context.SaveChangesAsync();
            }

            var deliveryDateUtc = deliveryDate.ToUniversalTime();
            var delivery = new Deliveries { ProviderId = provider.Id, Date = deliveryDateUtc };
            _context.Deliveries.Add(delivery);
            await _context.SaveChangesAsync();

            for (int i = 0; i < productTitles.Count; i++)
            {
                var productTitle = productTitles[i];
                var quantity = quantities[i];

                var product = await _context.Products.FirstOrDefaultAsync(p => p.Title == productTitle);
                if (product == null)
                {
                    var category = await _context.ProductCategories.FirstOrDefaultAsync();
                    if (category == null)
                    {

                    }

                    product = new Products { Title = productTitle, CategoryId = category.Id };
                    _context.Products.Add(product);
                    await _context.SaveChangesAsync();
                }

                if (!_context.Invoices.Any(i => i.DeliveryId == delivery.Id && i.ProductId == product.Id))
                {
                    var invoice = new Invoices { DeliveryId = delivery.Id, ProductId = product.Id, Quantity = quantity };
                    _context.Invoices.Add(invoice);
                    await _context.SaveChangesAsync();

                    var storage = await _context.Storages.FirstOrDefaultAsync(s => s.ProductId == product.Id);
                    if (storage != null)
                    {
                        storage.Remainder += quantity;
                    }
                    else
                    {
                        storage = new Storages { ProductId = product.Id, Remainder = quantity };
                        _context.Storages.Add(storage);
                    }
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Invoice");
        }
        public IActionResult Storages()
        {
            var categories = _context.ProductCategories.Include(c => c.Products).ThenInclude(p => p.Storages).ToList();
            return View(categories);
        }
        public IActionResult GetEmployeeOrdersDataByDateRange(DateTime startDate, DateTime endDate)
        {
            startDate = startDate.ToUniversalTime();
            endDate = endDate.ToUniversalTime();

            var employeeOrdersData = _context.Employees
                .Include(e => e.Orders)
                .Where(e => e.RoleId == 2 && e.Orders.Any(o => o.Date >= startDate && o.Date <= endDate && o.OrderStatusId == 1))
                .AsEnumerable()
                .Select(e => new
                {
                    employeeName = e.Initials,
                    completedOrders = e.Orders.Count(o => o.OrderStatusId == 1 && o.Date >= startDate && o.Date <= endDate),
                    totalAmount = e.Orders
                        .Where(o => o.Date >= startDate && o.Date <= endDate && o.OrderStatusId == 1)
                        .Sum(o => o.Total),
                    orderDates = e.Orders
                        .Where(o => o.Date >= startDate && o.Date <= endDate && o.OrderStatusId == 1)
                        .Select(o => o.Date.ToLocalTime())
                });

            return Json(employeeOrdersData);
        }
        public IActionResult EmployeeDiagramm()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(string initials, string login, int roleId)
        {
            var employee = new Employees
            {
                Initials = initials,
                Account = new Accounts { Login = login, Password = "defaultPassword" },
                RoleId = roleId
            };

            try
            {
                _context.Employees.Add(employee);
                _context.SaveChanges();

                employee.Id = Convert.ToInt32(employee.Id);

                return Json(new { success = true, employeeId = employee.Id });
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException;
                return Json(new { success = false, error = "An error occurred while creating the employee." });
            }
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return Json(new { success = false, error = "Employee not found" });
            }

            try
            {
                _context.Employees.Remove(employee);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException;
                return Json(new { success = false, error = "An error occurred while deleting the employee." });
            }
        }
        public IActionResult EmployeesTable()
        {
            var employees = _context.Employees.Include(e => e.Account).Include(e => e.Role).ToList();
            return View(employees);
        }
        public IActionResult Dishes()
        {
            var dishes = _context.Dishes
                .Include(d => d.Category)
                .ToList();

            var categories = _context.DishCategories.ToList();
            ViewBag.Categories = categories;

            return View(dishes);
        }


        [HttpPost]
        public IActionResult AddDish(Dishes dish)
        {
            var lastDishId = _context.Dishes.OrderByDescending(d => d.Id).FirstOrDefault()?.Id ?? 0;
            dish.Id = lastDishId + 1;

            var category = _context.DishCategories.FirstOrDefault(c => c.Id == dish.CategoryId);
            if (category != null)
            {
                dish.Category = category;

                _context.Dishes.Add(dish);
                _context.SaveChanges();
                return RedirectToAction("Dishes");
            }
            else
            {
                ModelState.AddModelError("Category.Id", "Выберите категорию");
                var model = _context.Dishes
                    .Include(d => d.Category)
                    .ToList();
                return View(" Dishes", model);
            }
        }
        [HttpPost]
        public IActionResult DeleteDish(int id)
        {
            var dish = _context.Dishes.Find(id);
            if (dish != null)
            {
                _context.Dishes.Remove(dish);
                _context.SaveChanges();
            }
            return RedirectToAction("Dishes");
        }
        public async Task<IActionResult> Orders(DateTime startDate, DateTime endDate)
        {
            var utcStartDate = startDate.ToUniversalTime();
            var utcEndDate = endDate.ToUniversalTime();

            var orders = await _context.Orders
                .Include(o => o.Employee)
                .Include(o => o.Client)
                .Include(o => o.OrderStatus)
                .Include(o => o.Payment)
                .Where(o => o.Date >= utcStartDate && o.Date <= utcEndDate)
                .ToListAsync();

            return View(orders);
        }
        public IActionResult Metrics()
        {
            return View();
        }
        public IActionResult GetRevenueDataByDate(DateTime startDate, DateTime endDate)
        {
            startDate = startDate.ToUniversalTime();
            endDate = endDate.ToUniversalTime();

            var revenueData = _context.Orders
                .Where(o => o.Date >= startDate && o.Date <= endDate)
                .GroupBy(o => new { Year = o.Date.Year, Month = o.Date.Month, Day = o.Date.Day })
                .Select(group => new
                {
                    Date = new DateTime(group.Key.Year, group.Key.Month, group.Key.Day),
                    Total = group.Sum(o => o.Total)
                });

            return Json(revenueData);
        }
        public IActionResult GetTopOrderedDishes()
        {
            var topDishes = _context.Dishes
                .Include(d => d.Checks)
                .OrderByDescending(d => d.Checks.Count)
                .Take(5)
                .Select(d => new
                {
                    dishTitle = d.Title,
                    orderedCount = d.Checks.Count
                });

            return Json(topDishes);
        }
        public IActionResult AbcAnalysis(DateTime startDate, DateTime endDate)
        {
            startDate = startDate.ToUniversalTime();
            endDate = endDate.ToUniversalTime();

            var dishesData = _context.Dishes
                .Include(d => d.Checks)
                .Include(d => d.Category)
                .Where(d => d.Checks.Any(c => c.Order.Date >= startDate && c.Order.Date <= endDate && c.Order.OrderStatusId == 1))
                .Select(d => new
                {
                    dishTitle = d.Title,
                    category = d.Category.Title,
                    income = d.Checks.Sum(c => c.Quantity * d.Cost),
                    revenue = d.Checks.Sum(c => c.Quantity * d.Cost) - d.Checks.Sum(c => c.Quantity * d.PrimeCost),
                    salesCount = d.Checks.Sum(c => c.Quantity)
                });

            return Json(dishesData);
        }
    }
}

