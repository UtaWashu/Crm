using Crm.Domain;
using Crm.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Crm.Controllers
{
    public class Waiter : Controller
    {
        private readonly CrmDbContext _context;

        public Waiter(CrmDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(DateTime startDate, DateTime endDate)
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
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
    public async Task<IActionResult> Create(Clients client)
        {
            var clients = await _context.Clients.ToListAsync();

            int newClientId = 1;
            while (clients.Any(c => c.Id == newClientId))
            {
                newClientId++;
            }
            int newLoyalityId = 1;

            client.Id = newClientId;
            client.LoyalityId = newLoyalityId;

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public IActionResult CreateOrder()
        {
            var employees = _context.Employees.ToList();
            var clients = _context.Clients.ToList();
            var categories = _context.DishCategories.ToList(); // Add this line to fetch categories

            var orders = _context.Orders
                .Where(o => o.OrderStatusId == 3)
                .ToList();
            var dishes = _context.Dishes.ToList();
            ViewBag.Dishes = dishes;

            ViewBag.Employees = employees;
            ViewBag.Clients = clients;
            ViewBag.Orders = orders;
            ViewBag.Categories = categories; // Add categories to ViewBag

            return View();
        }


        [HttpPost]
        public IActionResult CreateOrder(int employee, int client, double total, int peopleCount, int tableNumber, int paymentId)
        {
            var order = new Orders
            {
                EmployeeId = employee,
                ClientId = client,
                Total = total,
                PeopleCount = peopleCount,
                TableNumber = tableNumber,
                PaymentId = paymentId,
                Date = DateTime.UtcNow,
                OrderStatusId = 3
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> AddDishToOrder(Guid orderId, int dishId, int quantity)
        {
            var order = await _context.Orders.Include(o => o.Checks).FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes.Include(d => d.DishComponents).FirstOrDefaultAsync(d => d.Id == dishId);

            if (dish == null)
            {
                return NotFound();
            }

            var check = new Checks
            {
                OrderId = orderId,
                DishId = dishId,
                Quantity = quantity
            };

            // Calculate and update product quantity on storage
            foreach (var component in dish.DishComponents)
            {
                var storage = _context.Storages.FirstOrDefault(s => s.ProductId == component.ProductId);
                if (storage != null)
                {
                    storage.Remainder -= component.Quantity * quantity;
                    _context.Update(storage);
                }
            }

            _context.Checks.Add(check);

            order.Total += dish.Cost * quantity;

            await _context.SaveChangesAsync();

            return Ok();
        }


        public IActionResult GetOrderInfo(Guid orderId)
        {
            var order = _context.Orders
                .Include(o => o.Employee)
                .Include(o => o.Client)
                .Include(o => o.Payment)
                .Include(o => o.Checks).ThenInclude(c => c.Dish) // Include the Checks and related Dishes
                .FirstOrDefault(o => o.Id == orderId);

            if (order == null)
            {
                return NotFound();
            }

            var orderDishes = new Dictionary<int, int>(); // Dictionary to store dishId and quantity

            foreach (var check in order.Checks)
            {
                if (orderDishes.ContainsKey(check.DishId))
                {
                    orderDishes[check.DishId] += check.Quantity; // Increment quantity if the dish is already in the dictionary
                }
                else
                {
                    orderDishes.Add(check.DishId, check.Quantity); // Add the dish to the dictionary if it's not already present
                }
            }

            var orderInfo = new
            {
                employeeName = order.Employee.Initials,
                clientName = order.Client.Initials,
                total = order.Total,
                peopleCount = order.PeopleCount,
                tableNumber = order.TableNumber,
                paymentId = order.PaymentId,
                dishes = orderDishes.Select(kv => new { dishId = kv.Key, dishName = _context.Dishes.Find(kv.Key).Title, quantity = kv.Value }).ToList()
            };

            return Json(orderInfo);

        }


        public IActionResult GetDishes()
        {
            var dishes = _context.Dishes.ToList();
            ViewBag.AllDishes = dishes;
            return View();
        }

        [HttpPost]
        public IActionResult PayOrder(Guid orderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);

            if (order != null)
            {
                // Perform payment logic, for example:
                order.OrderStatusId = 1; // Paid status
                _context.SaveChanges();

                return Ok();
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult CancelOrder(Guid orderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);

            if (order != null)
            {
                // Perform cancellation logic, for example:
                order.OrderStatusId = 2; // Cancelled status
                _context.SaveChanges();

                return Ok();
            }

            return NotFound();
        }
        [HttpPost]
        public IActionResult UpdateDishQuantity(Guid orderId, int dishId, int quantity)
        {
            var check = _context.Checks.FirstOrDefault(c => c.OrderId == orderId && c.DishId == dishId);
            if (check != null)
            {
                int quantityDifference = quantity - check.Quantity;

                int a = check.Quantity - quantity;

                check.Quantity = quantity;

                // Обновление количества продуктов на складе
                var dish = _context.Dishes.Include(d => d.DishComponents).FirstOrDefault(d => d.Id == dishId);
                foreach (var component in dish.DishComponents)
                {
                    var storage = _context.Storages.FirstOrDefault(s => s.ProductId == component.ProductId);
                    if (storage != null)
                    {
                        storage.Remainder += component.Quantity * a;
                        _context.Update(storage);
                    }
                }

                _context.Checks.Update(check);
                _context.SaveChanges();

                return Ok();
            }

            return NotFound();
        }



        [HttpPost]
        public IActionResult RemoveDishFromOrder(Guid orderId, int dishId)
        {
            var check = _context.Checks.FirstOrDefault(c => c.OrderId == orderId && c.DishId == dishId);
            if (check != null)
            {
                var dish = _context.Dishes.Include(d => d.DishComponents).FirstOrDefault(d => d.Id == dishId);

                if (dish != null)
                {
                    foreach (var component in dish.DishComponents)
                    {
                        var storage = _context.Storages.FirstOrDefault(s => s.ProductId == component.ProductId);
                        if (storage != null)
                        {
                            // Вернуть продукты на склад
                            storage.Remainder += component.Quantity * check.Quantity;
                            _context.Update(storage);
                        }
                    }
                }

                _context.Checks.Remove(check);

                var order = _context.Orders.Include(o => o.Checks).FirstOrDefault(o => o.Id == orderId);
                if (order != null)
                {
                    order.Total -= _context.Dishes.Find(dishId).Cost * check.Quantity;
                    _context.Orders.Update(order);
                }

                _context.SaveChanges();
                return Ok();
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Check(Guid orderId)
        {
            var order = await _context.Orders
                .Include(o => o.Employee)
                .Include(o => o.Client)
                .Include(o => o.Payment)
                .Include(o => o.Checks)
                .ThenInclude(c => c.Dish)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return NotFound();
            }

            var orderDishes = order.Checks.Select(check => new
            {
                DishId = check.DishId,
                DishName = check.Dish.Title,
                Quantity = check.Quantity,
                Cost = check.Dish.Cost
            }).ToList();

            var orderReceipt = new
            {
                EmployeeName = order.Employee.Initials,
                OrderDate = order.Date,
                TotalCost = order.Total,
                Dishes = orderDishes
            };

            return View(orderReceipt);
        }

        public IActionResult Check()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetDishesByCategory(int categoryId)
        {
            var dishes = _context.Dishes
                .Where(dish => dish.CategoryId == categoryId)
                .Select(d => new { id = d.Id, title = d.Title })
                .ToList();

            return Json(dishes);
        }
        [HttpPost]
        public IActionResult UpdateTotalCost(Guid orderId, double dishCost, int dishQuantityChange)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);

            if (order != null)
            {
                order.Total += dishCost * dishQuantityChange;
                _context.SaveChanges();
                return Ok();
            }

            return NotFound();
        }

    }

}
