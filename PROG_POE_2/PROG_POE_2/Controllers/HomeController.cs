using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG_POE_2.Areas.Identity.Data;
using PROG_POE_2.Data;
using PROG_POE_2.Models;
using System.Diagnostics;

namespace PROG_POE_2.Controllers
{
    // The Authorize attribute specifies that access to this controller is restricted to authenticated users.
    [Authorize]
    public class HomeController : Controller
    {
        // ILogger is a service that allows logging of messages. It's used here to log messages from the HomeController.
        private readonly ILogger<HomeController> _logger;

        // UserManager is a service provided by ASP.NET Core Identity to manage users in the system.
        private readonly UserManager<ApplicationUser> _userManager;

        // Login_RegContext is the Entity Framework database context used to interact with the database.
        private readonly Login_RegContext _context;

        // The constructor uses dependency injection to get instances of ILogger, UserManager and Login_RegContext.
        // These instances are assigned to the private fields to be used in other methods in the controller.
        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, Login_RegContext context)
        {
            _logger = logger;
            this._userManager = userManager;
            this._context = context;
        }



        // Action for the FarmingHub view. When called, it will return the FarmingHub view.
        public IActionResult FarmingHub()
        {
            return View();
        }

        // Action for the Marketplace view. When called, it will return the Marketplace view.
        public IActionResult Marketplace()
        {
            return View();
        }

        // Action for the Education view. When called, it will return the Education view.
        public IActionResult Education()
        {
            return View();
        }

        // This action method is responsible for displaying all products of a specific farmer.
        // It takes a farmer ID as a parameter, retrieves all products of that farmer from the database,
        // and passes them to the FarmerProducts view.
        // It also retrieves the username of the farmer and passes it to the view using ViewBag.
        public async Task<IActionResult> AllProducts(int id)
        {
            var products = await _context.Products.Where(p => p.FarmerID == id).ToListAsync();
            if (products.Count > 0)
            {
                var farmerId = products[0].FarmerID;
                var farmer = await _context.Farmers.FindAsync(farmerId);
                ViewBag.FarmerUsername = farmer?.Username;
            }
            return View("FarmerProducts", products);
        }



        // This action method is responsible for displaying a list of farmers that were created by the currently logged-in employee.
        // It retrieves these farmers from the database and passes them to the view.
        public async Task<IActionResult> SelectFarmer()
        {
            // Get the ID of the currently logged-in user.
            var userId = _userManager.GetUserId(User);

            // Retrieve all farmers that were created by the currently logged-in user.
            var farmers = await _context.Farmers
                .Where(f => f.EmployeeID == userId)
                .ToListAsync();

            return View(farmers);
        }


        // This action method is responsible for displaying the products of a specific farmer.
        // It takes a farmer ID as a parameter, retrieves all products of that farmer from the database,
        // and passes them to the view.
        // It also retrieves the username of the farmer and passes it to the view using ViewBag.
        public async Task<IActionResult> FarmerProducts(int id)
        {
            var products = await _context.Products.Where(p => p.FarmerID == id).ToListAsync();
            if (products.Count > 0)
            {
                var farmerId = products[0].FarmerID;
                var farmer = await _context.Farmers.FindAsync(farmerId);
                ViewBag.FarmerUsername = farmer?.Username;
            }
            return View(products);
        }



        // GET: Farmers/ProductsByDate
        // This action method is responsible for displaying the products of a specific farmer within a specific date range.
        // It takes a farmer ID and optional start and end dates as parameters.
        // It retrieves all products of the farmer that were produced within the date range from the database and passes them to the FarmerProducts view.
        // It also retrieves the username of the farmer and passes it to the view using ViewBag.
        public async Task<IActionResult> ProductsByDate(int id, DateTime? startDate, DateTime? endDate)
        {
            // Start with a query that selects all products of the specified farmer.
            var query = _context.Products.Where(p => p.FarmerID == id);

            // If a start date is provided, add a condition to the query to only select products produced on or after the start date.
            if (startDate.HasValue)
            {
                query = query.Where(p => p.ProductionDate >= startDate.Value);
            }

            // If an end date is provided, add a condition to the query to only select products produced on or before the end date.
            if (endDate.HasValue)
            {
                query = query.Where(p => p.ProductionDate <= endDate.Value);
            }

            // Execute the query and get the list of products.
            var products = await query.ToListAsync();

            // If there are any products, find the farmer and pass their username to the view.
            if (products.Count > 0)
            {
                var farmerId = products[0].FarmerID;
                var farmer = await _context.Farmers.FindAsync(farmerId);
                ViewBag.FarmerUsername = farmer?.Username;
            }

            // Pass the list of products to the FarmerProducts view.
            return View("FarmerProducts", products);
        }




        // This action method is responsible for handling the GET request to the home page (Index view).
        // It retrieves the ID of the currently logged-in user and stores it in ViewData and TempData, which can be used to pass data to the view.
        public IActionResult Index()
        {
            // Get the ID of the currently logged-in user.
            // Store it in ViewData and TempData so it can be accessed in the view.
            ViewData["UserID"] = _userManager.GetUserId(this.User);
            TempData["UserID"] = _userManager.GetUserId(this.User);

            // Return the Index view.
            return View();
        }

        // This action method is responsible for handling the GET request to the Privacy view.
        public IActionResult Privacy()
        {
            // Return the Privacy view.
            return View();
        }

        // This action method is responsible for handling the GET request to the Error view.
        // It creates a new ErrorViewModel with the current request ID and passes it to the view.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Create a new ErrorViewModel with the current request ID.
            // If there is no current Activity (request), use the TraceIdentifier from the HttpContext.
            // Pass the ErrorViewModel to the Error view.
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // This action method is responsible for logging out the user.
        // It clears the session and redirects to the home page (Index view).
        [AllowAnonymous]
        public IActionResult Logout()
        {
            // Clear the session.
            HttpContext.Session.Clear();

            // Redirect to the Index action of the Home controller.
            return RedirectToAction("Index", "Home");
        }

    }
}
