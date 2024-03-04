using BicycleStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace BicycleStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly bicycleStoreContext _dbContext;

        public HomeController(ILogger<HomeController> logger, bicycleStoreContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        private (string UserRole, int? UserId) GetUserRoleAndId(string username, string password)
        {
            // Check if the user is an employee
            var employee = _dbContext.employees.FirstOrDefault(e => e.Username == username && e.Password == password);
            if (employee != null)
            {
                return ("Employee", employee.id);
            }

            // Check if the user is an admin
            var admin = _dbContext.admins.FirstOrDefault(a => a.Username == username && a.Password == password);
            if (admin != null)
            {
                return ("Admin", admin.id);
            }

            // Return null or any other value to indicate authentication failure
            return (null, null);
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [HttpPost]
        public IActionResult Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var (userRole, userId) = GetUserRoleAndId(model.Username, model.Password);

                if (userRole != null)
                {
                    HttpContext.Session.SetString("UserRole", userRole);
                    if (userId.HasValue)
                    {
                        HttpContext.Session.SetInt32("UserId", userId.Value);
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or password");
                }
            }

            return View(model);
        }


        public IActionResult SalesReport()
        {
            return View();
        }

        [HttpPost]
        public List<object> GetSalesData()
        {
            List<object> data = new List<object>();

            // Group rentals by month and calculate the total sales for each month
            var monthlySales = _dbContext.rentals
                .GroupBy(r => r.RentalStartDay.Month)
                .Select(g => new { Month = g.Key, TotalSales = g.Sum(r => r.RentalFee) })
                .OrderBy(g => g.Month)
                .ToList();

            // Extract the month names and sales data
            List<string> labels = monthlySales.Select(g => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Month)).ToList();
            List<decimal> salesData = monthlySales.Select(g => g.TotalSales).ToList();

            data.Add(labels);
            data.Add(salesData);

            return data;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}