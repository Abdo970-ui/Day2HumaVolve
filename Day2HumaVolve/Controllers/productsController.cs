using Day2HumaVolve.Data;
using Day2HumaVolve.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Day2HumaVolve.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class productsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public productsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet] // AsNoTracking()
        public ActionResult GetAllProducts()
        {
            if (_context.Products.AsNoTracking().Any())
            {
                return Ok(_context.Products.AsNoTracking().ToList());
            }
            return NotFound();
        }
        [HttpGet("GetById")] // Select()
        public ActionResult GetProductById([FromQuery] int id)
        {
            if (_context.Products.AsNoTracking().Where(p => p.Id == id).Any())
                return Ok(_context.Products.AsNoTracking().Where(p => p.Id == id));
            return NotFound();
        }
        [HttpGet("WithCategories")] // N+1 Problem Solution
        public ActionResult GetAllProductsByCategory()
        {
            if (_context.Products.AsNoTracking().Any())
            {

                var products = _context.Products.AsNoTracking()
                    .Select(p => new
                    {
                        p.Id,
                        p.Name,
                        p.Price,
                        CategoryName = p.Category.Name
                    }).ToList();
                return Ok(products);

            }

            return NotFound();

        }
        [HttpPut("UpdateById")]
        public ActionResult UpdateById(
           [FromQuery] int id,
           [FromBody] ProductDTO product)
        {
            if (product.RowVersion == null || product.RowVersion.Length == 0)
            {
                return BadRequest("RowVersion is required.");
            }

            var existingProduct = _context.Products
                .FirstOrDefault(p => p.Id == id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            // Use the version supplied by the client for concurrency checking.
            _context.Entry(existingProduct)
                .Property(p => p.RowVersion)
                .OriginalValue = product.RowVersion;

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.CategoryId = product.CategoryId;

            try
            {
                _context.SaveChanges();
                return Ok(existingProduct);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("The product was modified by another user. Fetch the latest version and try again.");
            }
        }
    }
}
