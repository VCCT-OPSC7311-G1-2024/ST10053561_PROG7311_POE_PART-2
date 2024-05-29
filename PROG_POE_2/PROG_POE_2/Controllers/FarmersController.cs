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
        // UserManager is a service provided by ASP.NET Core Identity to manage users in the system.
        private readonly UserManager<ApplicationUser> _userManager;

        // Login_RegContext is the Entity Framework database context used to interact with the database.
        private readonly Login_RegContext _context;

        // The constructor uses dependency injection to get instances of UserManager and Login_RegContext.
        public FarmersController(UserManager<ApplicationUser> userManager, Login_RegContext context)
        {
            _context = context;
            this._userManager = userManager;
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

        // GET: Farmers
        // This action method is responsible for displaying the list of farmers.
        // It first checks if there is a farmerId stored in the session.
        // If there is, it checks if a farmer with that ID exists in the database.
        // It then passes a boolean value to the view indicating whether the current user is a farmer.
        // Finally, it passes the list of all farmers to the view.
        public async Task<IActionResult> Index()
        {
            var farmerId = HttpContext.Session.GetInt32("FarmerId");
            var isFarmer = farmerId.HasValue && await _context.Farmers.AnyAsync(f => f.Id == farmerId.Value);
            ViewBag.IsFarmer = isFarmer;
            return View(await _context.Farmers.ToListAsync());
        }


        // GET: Farmers/Display
        // This action method is responsible for displaying the list of farmers.
        // It retrieves all farmers from the database that were created by the currently logged-in employee and passes them to the view.
        public async Task<IActionResult> Display()
        {
            // Get the ID of the currently logged-in employee.
            var userId = _userManager.GetUserId(User);

            // Retrieve all farmers that were created by the currently logged-in employee.
            var farmers = await _context.Farmers
                .Where(f => f.EmployeeID == userId)
                .ToListAsync();

            return View(farmers);
        }



        // GET: Farmers/Details/5
        // This action method is responsible for displaying the details of a specific farmer.
        // It takes an optional farmer ID as a parameter.
        // If no ID is provided, it returns a 404 error.
        // If an ID is provided, it retrieves the farmer with that ID from the database.
        // If no farmer with that ID exists, it returns a 404 error.
        // If a farmer with that ID exists, it passes the farmer to the view.
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
        // This action method is responsible for handling the GET request to create a new farmer.
        // It simply returns the Create view.
        public IActionResult Create()
        {
            return View();
        }

        // POST: Farmers/Create
        // This action method is responsible for handling the POST request to create a new farmer.
        // It takes a Farmer object as a parameter, which is populated with the form data from the Create view.
        // It then validates the model, adds the new farmer to the database, and redirects to the Home Index view.
        // If the model is not valid, it returns the Create view with the validation errors.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,LastName,Location,Contact,Date,Username,Password")] Farmer farmer)
        {
            // Get the current user id from the User object and assign it to the EmployeeID property of the farmer.
            var userId = _userManager.GetUserId(this.User);

            if (userId == null)
            {
                // No user is logged in, redirect to the Login view.
                return RedirectToAction("Login", "Account");
            }

            // Assign the current user id to the EmployeeID property of the farmer.
            farmer.EmployeeID = userId;

            // Hash the password using the PasswordHasher service and assign it to the PasswordHash property of the farmer.
            var passwordHasher = new PasswordHasher<Farmer>();
            farmer.PasswordHash = passwordHasher.HashPassword(farmer, farmer.Password);

            // Clear the model state to remove any validation errors.
            ModelState.Clear();

            // Manually validate the model.
            TryValidateModel(farmer);

            if (ModelState.IsValid)
            {
                // Set the Date property to the date part only.
                farmer.Date = farmer.Date.Date;

                // Add the new farmer to the database and save the changes.
                _context.Add(farmer);
                await _context.SaveChangesAsync();

                // Redirect to the Home Index view.
                return RedirectToAction(nameof(Index), "Home");
            }

            // If the model is not valid, add the validation errors to the model state and return the Create view.
            var errors = ModelState.SelectMany(x => x.Value.Errors.Select(e => e.ErrorMessage));
            foreach (var error in errors)
            {
                ModelState.AddModelError("", error);
            }
            return View(farmer);
        }



        // GET: Farmers/Edit/5
        // This action method is responsible for handling the GET request to edit a farmer.
        // It takes an optional farmer ID as a parameter.
        // If no ID is provided, it returns a 404 error.
        // If an ID is provided, it retrieves the farmer with that ID from the database.
        // If no farmer with that ID exists, it returns a 404 error.
        // If a farmer with that ID exists, it passes the farmer to the Edit view.
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
        // This action method is responsible for handling the POST request to edit a farmer.
        // It takes a farmer ID and a Farmer object as parameters.
        // The Farmer object is populated with the form data from the Edit view.
        // It first checks if the ID in the URL matches the ID of the Farmer object.
        // If they don't match, it returns a 404 error.
        // If they match, it validates the model, updates the farmer in the database, and redirects to the Index view.
        // If the model is not valid, it returns the Edit view with the validation errors.
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
        // This action method is responsible for handling the GET request to delete a farmer.
        // It takes an optional farmer ID as a parameter.
        // If no ID is provided, it returns a 404 error.
        // If an ID is provided, it retrieves the farmer with that ID from the database.
        // If no farmer with that ID exists, it returns a 404 error.
        // If a farmer with that ID exists, it passes the farmer to the Delete view.
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
        // This action method is responsible for handling the POST request to delete a farmer.
        // It takes a farmer ID as a parameter.
        // It retrieves the farmer with that ID from the database.
        // If a farmer with that ID exists, it removes the farmer from the database and saves the changes.
        // It then redirects to the Display view.
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

        // This method checks if a farmer with the given ID exists in the database.
        private bool FarmerExists(int id)
        {
            return _context.Farmers.Any(e => e.Id == id);
        }
    }

}
