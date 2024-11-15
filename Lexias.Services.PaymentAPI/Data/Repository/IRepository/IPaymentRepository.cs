using Lexias.Services.PaymentAPI.Models;

namespace Lexias.Services.PaymentAPI.Data.Repository.IRepository
{
    public interface IPaymentRepository
    {
        Task AddPaymentAsync(Payment payment);
        Task<Payment> GetPaymentByIdAsync(string paymentId);
        Task<IEnumerable<Payment>> GetAllPaymentsAsync();
        Task UpdatePaymentAsync(Payment payment);
        Task DeletePaymentAsync(string paymentId);
    }
}
