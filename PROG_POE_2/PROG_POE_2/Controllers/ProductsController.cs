using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PROG_POE_2.Data;
using PROG_POE_2.Models;

namespace PROG_POE_2.Controllers
{
	public class ProductsController : Controller
	{
		private readonly Login_RegContext _context;

		public ProductsController(Login_RegContext context)
		{
			_context = context;
		}

		// GET: Products
        public async Task<IActionResult> Display()
        {
            return View(await _context.Products.ToListAsync());
        }


        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var product = await _context.Products
				.FirstOrDefaultAsync(m => m.ProductID == id);
			if (product == null)
			{
				return NotFound();
			}

			return View(product);
		}

		// GET: Products/Create
		public IActionResult Create()
		{
			return View();
		}

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,Name,Type,description,ProductionDate,FarmerID")] Product product)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the farmer's ID from the session
                var farmerId = HttpContext.Session.GetInt32("FarmerId");
                if (!farmerId.HasValue)
                {
					// Handle the case where the farmer is not logged in
					return Redirect("/Identity/Account/Login");
                }

                // Assign the current farmer's ID to the FarmerID property of the product
                product.FarmerID = farmerId.Value;

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Display));
            }


            // If model state is not valid, collect all errors
            var errors = ModelState.SelectMany(x => x.Value.Errors.Select(e => e.ErrorMessage));
            foreach (var error in errors)
            {
                ModelState.AddModelError("", error);
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var product = await _context.Products.FindAsync(id);
			if (product == null)
			{
				return NotFound();
			}
			return View(product);
		}

		// POST: Products/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,Name,Type,description,ProductionDate,FarmerID")] Product product)
        {
            if (id != product.ProductID)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(product);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ProductExists(product.ProductID))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Display));
			}
			return View(product);
		}

		// GET: Products/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var product = await _context.Products
				.FirstOrDefaultAsync(m => m.ProductID == id);
			if (product == null)
			{
				return NotFound();
			}

			return View(product);
		}

		// POST: Products/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var product = await _context.Products.FindAsync(id);
			if (product != null)
			{
				_context.Products.Remove(product);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Display));
		}

		private bool ProductExists(int id)
		{
			return _context.Products.Any(e => e.ProductID == id);
		}
	}
}
