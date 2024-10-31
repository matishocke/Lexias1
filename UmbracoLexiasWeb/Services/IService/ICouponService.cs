using UmbracoLexiasWeb.Models;

namespace UmbracoLexiasWeb.Services.IService
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
