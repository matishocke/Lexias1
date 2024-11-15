using Lexias.Services.OrderAPI.Data.Repository.IRepository;
using Lexias.Services.OrderAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Lexias.Services.OrderAPI.Data.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }




        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            var foundAllOrders = await _context.Orders.Include(o => o.OrderItemsList).ToListAsync();
            return foundAllOrders;
        }

        public async Task<Order> GetOrderByIdAsync(string orderId)
        {
            var foundOrder = await _context.Orders
                .Include(o => o.OrderItemsList)
                .FirstOrDefaultAsync(w => w.OrderId == orderId);

            if (foundOrder == null)
            {
                throw new KeyNotFoundException($"Order with ID {orderId} not found.");
            }

            return foundOrder;
        }

        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(string orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {orderId} not found.");
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}
