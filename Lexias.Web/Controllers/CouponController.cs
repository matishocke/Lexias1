using Lexias.Web.Models;
using Lexias.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace Lexias.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;
        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }




        public async Task<IActionResult> CouponIndex()
        {
            var listOfAllCoupon = await _couponService.GetAllCouponAsync();
            if (listOfAllCoupon == null) { return NotFound(); }


            return View(listOfAllCoupon);
        }







        //Create Ready the page [Get]
        public async Task<IActionResult> CouponCreate()
        {
            return View();
        }
        //Create  [Post]
        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponDto couponDto)
        {
            // Check if the submitted data is VALID
            if (ModelState.IsValid)
            {
                await _couponService.CreateCouponAsync(couponDto);

                // If deletion is successful, redirect to the index page
                return RedirectToAction("CouponIndex");
            }
            else
            {
                ModelState.AddModelError("", "");
            }

            return View(couponDto);
        }








        //DELETE Ready the page [Get]
        public async Task<IActionResult> CouponDelete(int couponId)
        {
            var getCoupon = await _couponService.GetCouponByIdAsync(couponId);
            if (getCoupon == null)
            {
                return NotFound();
            }
            return View(getCoupon);
        }
        //DELETE [Post]
        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponDto couponDto)
        {
            if (couponDto == null || couponDto.CouponId <= 0)
            {
                return NotFound();
            }
            try
            {
                await _couponService.DeleteCouponAsync(couponDto.CouponId);
                return RedirectToAction("CouponIndex");
            }
            catch (Exception ex)
            {
                // Log the exception, return an error page or a message
                // Example: return a custom error view with a message
                ModelState.AddModelError("", "An error occurred while deleting the coupon.");
                return View(couponDto); // Returning to the same page
            }

        }
    }
}
