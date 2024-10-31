using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;
using UmbracoLexiasWeb.Models;
using UmbracoLexiasWeb.Models.ViewModels;
using UmbracoLexiasWeb.Services.IService;

namespace UmbracoLexiasWeb.Controller
{
    public class CouponSurfaceController : SurfaceController
    {
        private readonly ICouponService _couponService;
        public CouponSurfaceController(
            IUmbracoContextAccessor umbracoContextAccessor,
            IUmbracoDatabaseFactory databaseFactory,
            ServiceContext services,
            AppCaches appCaches,
            IProfilingLogger profilingLogger,
            IPublishedUrlProvider publishedUrlProvider,
            ICouponService couponService) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _couponService = couponService;
        }



        //// GET: Retrieve and display all coupons
        //public async Task<IActionResult> CouponIndex()
        //{
        //    var couponsDto = await _couponService.GetAllCouponAsync();

        //    //Select?????? You already used Select correctly to map CouponDto to CouponViewModel.
        //    var couponShowViewModel = couponsDto.Select(x => new CouponViewModel
        //    {
        //        CouponId = x.CouponId,
        //        CouponCode = x.CouponCode,
        //        DiscountAmount = x.DiscountAmount,
        //        MinAmount = x.MinAmount,
        //    }).ToList();


        //    return View("CouponIndexTemp", couponShowViewModel); // Make sure you have a corresponding view
        //}









        //[HttpGet]
        //public IActionResult CouponCreate()
        //{
        //    var model = new CouponViewModel();
        //    return PartialView("CouponSurface/CouponCreate", model); // Return to the current page to display the form
        //}

        // POST: Handle the form submission to create a coupon
        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponViewModel couponViewModel)
        {
            if (ModelState.IsValid)
            {
                var couponDto = new CouponDto
                {
                    CouponId = couponViewModel.CouponId,
                    CouponCode = couponViewModel.CouponCode,
                    DiscountAmount = couponViewModel.DiscountAmount,
                    MinAmount = couponViewModel.MinAmount
                };

                await _couponService.CreateCouponAsync(couponDto);
                TempData["Success"] = "Coupon created successfully!";

                return Redirect("/couponindexpage"); // Redirect back to the couponindexpage
            }

            return CurrentUmbracoPage(); // Return to the current page with validation errors
        }










        // GET: Render the delete confirmation page
        [HttpGet]
        public async Task<IActionResult> CouponDelete(int couponId)
        {
            var coupon = await _couponService.GetCouponByIdAsync(couponId);
            if (coupon == null)
            {
                return NotFound(); // Return 404 if coupon not found
            }

            // Pass the coupon to the view
            var couponViewModel = new CouponViewModel
            {
                CouponId = coupon.CouponId,
                CouponCode = coupon.CouponCode,
                DiscountAmount = coupon.DiscountAmount,
                MinAmount = coupon.MinAmount,
            };

            return View("~/Views/CouponDelete.cshtml", couponViewModel); // Ensure this view is where you want to render the main view
        }

        // POST: Handle the form submission to delete a coupon
        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponViewModel couponViewModel)
        {
            if (couponViewModel == null || couponViewModel.CouponId <= 0)
            {
                TempData["Error"] = "Coupon could not be found";
                return NotFound(); // Return 404 if coupon is invalid
            }


            var couponDto = new CouponDto
            {
                CouponId = couponViewModel.CouponId,
                CouponCode = couponViewModel.CouponCode,
                DiscountAmount = couponViewModel.DiscountAmount,
                MinAmount = couponViewModel.MinAmount
            };


            await _couponService.DeleteCouponAsync(couponDto.CouponId);
            TempData["Success"] = "Coupon deleted successfully!";
            return Redirect("/couponindexpage"); // Redirect back to the current page
        }

    }
}
