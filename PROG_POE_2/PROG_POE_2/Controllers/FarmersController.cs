using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PROG_POE_2.Areas.Identity.Data;
using PROG_POE_2.Data;
using PROG_POE_2.Models;

namespace PROG_POE_2.Controllers
{
    public class FarmersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Login_RegContext _context;

        public FarmersController(UserManager<ApplicationUser> userManager, Login_RegContext context)
        {
            _context = context;
            this._userManager = userManager;
        }

        public IActionResult FarmingHub()
        {
            return View();
        }

        public IActionResult Marketplace()
        {
            return View();
        }

        public IActionResult Education()
        {
            return View();
        }


        // GET: Farmers
        public async Task<IActionResult> Index()
        {
            var farmerId = HttpContext.Session.GetInt32("FarmerId");
            var isFarmer = farmerId.HasValue && await _context.Farmers.AnyAsync(f => f.Id == farmerId.Value);
            ViewBag.IsFarmer = isFarmer;
            return View(await _context.Farmers.ToListAsync());
        }


        // GET: Farmers/Display
        public async Task<IActionResult> Display()
        {
            return View(await _context.Farmers.ToListAsync());
        }


        // GET: Farmers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farmer = await _context.Farmers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (farmer == null)
            {
                return NotFound();
            }

            return View(farmer);
        }

        // GET: Farmers/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,LastName,Location,Contact,Date,Username,Password")] Farmer farmer)
        {
            // Get the current user id from LoginUser and assign it to the EmployeeID property.
            var userId = _userManager.GetUserId(this.User);

            if (userId == null)
            {
                Console.WriteLine("No User is logged in");

                // No user is logged in, handle this case
                return RedirectToAction("Login", "Account");
            }

            Console.WriteLine("Current User ID : " + userId);

            // Get the current user id and assign it to the EmployeeID property.
            farmer.EmployeeID = userId;

            // Hash the password
            var passwordHasher = new PasswordHasher<Farmer>();
            farmer.PasswordHash = passwordHasher.HashPassword(farmer, farmer.Password);

            // Clear the model state
            ModelState.Clear();

            // Manually validate the model
            TryValidateModel(farmer);

            if (ModelState.IsValid)
            {
                // Set the Date property to the date part only.
                farmer.Date = farmer.Date.Date;

                _context.Add(farmer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), "Home");
            }
            var errors = ModelState.SelectMany(x => x.Value.Errors.Select(e => e.ErrorMessage));
            foreach (var error in errors)
            {
                ModelState.AddModelError("", error);
            }
            return View(farmer);
        }

        // GET: Farmers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farmer = await _context.Farmers.FindAsync(id);
            if (farmer == null)
            {
                return NotFound();
            }
            return View(farmer);
        }

        // POST: Farmers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,LastName,Location,Contact,Date")] Farmer farmer)
        {

            if (id != farmer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(farmer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FarmerExists(farmer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(farmer);
        }

        // GET: Farmers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farmer = await _context.Farmers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (farmer == null)
            {
                return NotFound();
            }

            return View(farmer);
        }

        // POST: Farmers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var farmer = await _context.Farmers.FindAsync(id);
            if (farmer != null)
            {
                _context.Farmers.Remove(farmer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Display));
        }

        private bool FarmerExists(int id)
        {
            return _context.Farmers.Any(e => e.Id == id);
        }
    }

}
