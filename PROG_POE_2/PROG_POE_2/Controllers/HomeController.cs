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
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Login_RegContext _context;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, Login_RegContext context)
        {
            _logger = logger;
            this._userManager = userManager;
            this._context = context;
        }

        public async Task<IActionResult> AllProducts(int id)
        {
            var products = await _context.Products.Where(p => p.FarmerID == id).ToListAsync();
            return View("FarmerProducts", products);
        }


        public async Task<IActionResult> SelectFarmer()
        {
            var farmers = await _context.Farmers.ToListAsync();
            return View(farmers);
        }

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
        public async Task<IActionResult> ProductsByDate(int id, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Products.Where(p => p.FarmerID == id);

            if (startDate.HasValue)
            {
                query = query.Where(p => p.ProductionDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(p => p.ProductionDate <= endDate.Value);
            }

            return View("FarmerProducts", await query.ToListAsync());
        }





        public IActionResult Index()
        {
            //This allows to get the current user id
            ViewData["UserID"] = _userManager.GetUserId(this.User);
            TempData["UserID"] = _userManager.GetUserId(this.User);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [AllowAnonymous]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}
