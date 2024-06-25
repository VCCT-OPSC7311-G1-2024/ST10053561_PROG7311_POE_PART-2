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
using Microsoft.AspNetCore.Http;
using System.IO;

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
            // Retrieve the farmer's ID from the session
            var farmerId = HttpContext.Session.GetInt32("FarmerId");
            if (!farmerId.HasValue)
            {
                // Handle the case where the farmer is not logged in
                return Redirect("/Identity/Account/Login");
            }

            // Filter the products based on the farmer's ID
            var products = await _context.Products
                .Where(p => p.FarmerID == farmerId.Value)
                .ToListAsync();

            return View(products);
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

        // Modify the Create action to include IFormFile parameter for the image
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,Name,Type,description,ProductionDate,FarmerID,Image")] Product product, IFormFile imageUpload)
        {
            if (imageUpload == null || imageUpload.Length == 0)
            {
                ModelState.AddModelError("ImageUpload", "The Product Image field is required.");
            }
            else
            {
                // If there's an uploaded file, remove the Image property from ModelState validation
                ModelState.Remove("Image");
            }

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

                if (imageUpload != null && imageUpload.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imageUpload.CopyToAsync(memoryStream);
                        product.Image = memoryStream.ToArray();
                    }
                }

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
        // This action method is responsible for handling the GET request to edit a product.
        // It takes an optional product ID as a parameter.
        // If no ID is provided, it returns a 404 error.
        // If an ID is provided, it retrieves the product with that ID from the database.
        // If no product with that ID exists, it returns a 404 error.
        // If a product with that ID exists, it passes the product to the Edit view.
        public async Task<IActionResult> Edit(int? id)
        {
            // Check if the ID is null. If it is, return a 404 error.
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve the product with the specified ID from the database.
            var product = await _context.Products.FindAsync(id);

            // Check if the product exists. If it doesn't, return a 404 error.
            if (product == null)
            {
                return NotFound();
            }

            // If the product exists, pass it to the Edit view.
            return View(product);
        }


        // POST: Products/Edit/5
        // This action method is responsible for handling the POST request to edit a product.
        // It takes a product ID and a Product object as parameters.
        // The Product object is bound to the posted form values.
        // If the ID in the URL does not match the ID of the product, it returns a 404 error.
        // If the model state is valid, it retrieves the existing product from the database and updates its properties.
        // It then saves the changes to the database and redirects to the Display view.
        // If the model state is not valid, it returns the Edit view with the current product.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,Name,Type,description,ProductionDate")] Product product, IFormFile imageUpload)
        {

            // Check if the ID in the URL matches the ID of the product. If not, return a 404 error.
            if (id != product.ProductID)
            {
                return NotFound();
            }

            if (imageUpload == null || imageUpload.Length == 0)
            {
                ModelState.AddModelError("ImageUpload", "The Product Image field is required.");
            }
            else
            {
                // If there's an uploaded file, remove the Image property from ModelState validation
                ModelState.Remove("Image");
            }

            // Check if the model state is valid.
            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the existing product from the database.
                    var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == id);
                    if (existingProduct == null)
                    {
                        return NotFound();
                    }

                    // Retrieve the farmer's ID from the session.
                    var farmerId = HttpContext.Session.GetInt32("FarmerId");
                    if (!farmerId.HasValue)
                    {
                        // If the farmer is not logged in, redirect to the login page.
                        return Redirect("/Identity/Account/Login");
                    }

                    // Update the properties of the existing product.
                    existingProduct.Name = product.Name;
                    existingProduct.Type = product.Type;
                    existingProduct.description = product.description;
                    existingProduct.ProductionDate = product.ProductionDate;
                    existingProduct.FarmerID = farmerId.Value;


                    if (imageUpload != null && imageUpload.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await imageUpload.CopyToAsync(memoryStream);
                            existingProduct.Image = memoryStream.ToArray();
                        }
                    }

                    // Save the changes to the database.
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // If the product does not exist, return a 404 error.
                    // Otherwise, rethrow the exception.
                    if (!ProductExists(product.ProductID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                // Redirect to the Display view.
                return RedirectToAction(nameof(Display));
            }

            // If model state is not valid, collect all errors
            var errors = ModelState.SelectMany(x => x.Value.Errors.Select(e => e.ErrorMessage));
            foreach (var error in errors)
            {
                ModelState.AddModelError("", error);
            }

            // If the model state is not valid, return the Edit view with the current product.
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
        // This action method is responsible for handling the POST request to delete a product.
        // It takes a product ID as a parameter.
        // It retrieves the product with that ID from the database and removes it.
        // It then saves the changes to the database and redirects to the Display view.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Retrieve the product with the specified ID from the database.
            var product = await _context.Products.FindAsync(id);

            // If the product exists, remove it from the database.
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            // Save the changes to the database.
            await _context.SaveChangesAsync();

            // Redirect to the Display view.
            return RedirectToAction(nameof(Display));
        }

        // This helper method checks if a product with a specified ID exists in the database.
        // It takes a product ID as a parameter and returns true if a product with that ID exists, false otherwise.
        private bool ProductExists(int id)
        {
            // Check if any product in the database has the specified ID.
            return _context.Products.Any(e => e.ProductID == id);
        }


    }
}
