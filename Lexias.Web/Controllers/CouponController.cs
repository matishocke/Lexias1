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
            try
            {
                var listOfAllCoupon = await _couponService.GetAllCouponAsync();

                if (listOfAllCoupon != null)
                {
                    return View(listOfAllCoupon);
                }
                TempData["Error"] = "No coupons found";
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while retrieving coupons";
                return StatusCode(500); // Internal Server Error
            }
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
                try
                {
                    await _couponService.CreateCouponAsync(couponDto);

                    TempData["Success"] = "Coupon created successfully!";

                    // If deletion is successful, redirect to the index page
                    return RedirectToAction("CouponIndex");
                }
                catch (Exception ex) 
                {
                    TempData["Error"] = "An error occurred while creating the coupon.";

                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid coupon data. Please correct the errors and try again.");
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
                TempData["Error"] = "Coupon could not be found";
                return NotFound();
            }
            try
            {
                await _couponService.DeleteCouponAsync(couponDto.CouponId);
                TempData["Success"] = "Coupon Deleted Successfully";
                return RedirectToAction("CouponIndex");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Coupon Not Deleted";
                return RedirectToAction("CouponIndex");
            }

        }
    }
}
