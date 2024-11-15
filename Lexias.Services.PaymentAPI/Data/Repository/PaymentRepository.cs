using Lexias.Services.PaymentAPI.Data.Repository.IRepository;
using Lexias.Services.PaymentAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Lexias.Services.PaymentAPI.Data.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContextPayment _context;

        public PaymentRepository(AppDbContextPayment context)
        {
            _context = context;
        }




        public async Task AddPaymentAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
        }

        public async Task<Payment> GetPaymentByIdAsync(string paymentId)
        {
            var foundPeyment = await _context.Payments.FirstOrDefaultAsync(p => p.PaymentId == paymentId);
            return foundPeyment;
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            var foundPayments = await _context.Payments.ToListAsync();
            return foundPayments;
        }

        public async Task UpdatePaymentAsync(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePaymentAsync(string paymentId)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.PaymentId == paymentId);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
