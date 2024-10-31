using static Umbraco.Cms.Core.Constants.Validation;
using System.ComponentModel.DataAnnotations;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace UmbracoLexiasWeb.Models.ViewModels
{
    public class CouponViewModel
    {
        public int CouponId { get; set; }


        [Required]
        [StringLength(10, ErrorMessage = "Coupon code cannot be longer than 10 characters.")]
        public string CouponCode { get; set; }



        [Range(0, double.MaxValue, ErrorMessage = "Discount amount must be a positive value.")]
        public double DiscountAmount { get; set; }



        [Range(0, double.MaxValue, ErrorMessage = "Minimum amount must be a positive value.")]
        public int MinAmount { get; set; }



        //public IPublishedContent Content { get; set; } // This is for the current Umbraco page content

    }
}
