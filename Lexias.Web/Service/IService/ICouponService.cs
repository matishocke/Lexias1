using Lexias.Web.Models;

namespace Lexias.Web.Service.IService
{
    public interface ICouponService
    {
        Task<CouponDto?> GetCouponCodeAsync(string couponCode);
        Task<IEnumerable<CouponDto>?> GetAllCouponAsync();
        Task<CouponDto?> GetCouponByIdAsync(int id);
        Task? CreateCouponAsync(CouponDto couponDto);
        Task? UpdateCouponAsync(CouponDto couponDto);
        Task? DeleteCouponAsync(int id);
    }
}
