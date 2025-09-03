using FirstWebApi.Data;
using FirstWebApi.Models;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace FirstWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ProductsController));

        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/products
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _context.Products.ToListAsync();
            _log.Info("Fetching all products");
            return Ok(products);
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            //var product = await _context.Products.FindAsync(id);
            //_log.Info($"Fetched product with id {id}");
            //return product == null ? NotFound() : Ok(product);

            try
            {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                _log.Warn($"Product with id {id} not found");
                return NotFound();
            }
            else
            {
                _log.Info($"Fetched product with id {id}");
                return Ok(product);
            }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return BadRequest();
            }
        }

        // POST: api/products
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            _log.Info($"Added product: {product}");
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Product product)
        {
            if (id != product.Id) return BadRequest("ID mismatch");

            var existing = await _context.Products.FindAsync(id);
            if (existing == null)
            {
                _log.Warn($"Cannot update. Product with id {id} not found");
                return NotFound();
            }

            // update fields
            existing.Name = product.Name;
            existing.Price = product.Price;
            existing.Stock = product.Stock;

            await _context.SaveChangesAsync();
            _log.Info($"Updated product with id {id} to {product}");
            return Ok(existing);
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                _log.Warn($"Cannot delete. Product with id {id} not found");
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            _log.Info($"Deleted product: {id}");
            return NoContent();
        }
    }
}
