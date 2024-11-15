using Lexias.Services.WarehouseAPI.Data.Repository.IRepository;
using Lexias.Services.WarehouseAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Lexias.Services.WarehouseAPI.Data.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContextWarehouse _context;

        public ProductRepository(AppDbContextWarehouse context)
        {
            _context = context;
        }





        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(string productId)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(string productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}