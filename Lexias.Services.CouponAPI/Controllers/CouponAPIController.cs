using AutoMapper;
using Lexias.Services.CouponAPI.Data;
using Lexias.Services.CouponAPI.Models.Dto;
using Lexias.Services.CouponAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Lexias.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        //just for making a good architecture so we can return all object in one dto(ResponseDto) class

        private IMapper _mapper;


        public CouponAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<CouponDto>> Get()
        {
            try
            {
                IEnumerable<Coupon> foundAllCoupons = _db.Coupons.ToList();
                if (foundAllCoupons == null || !foundAllCoupons.Any())
                {
                    return NotFound();  // Return 404 if no coupons are found
                }

                IEnumerable<CouponDto> mappCouponToCouponDto = _mapper.Map<IEnumerable<CouponDto>>(foundAllCoupons);
                return Ok(mappCouponToCouponDto.ToList());

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the data.");
            }
        }





        //In absence of [Route("{id:int}")], the default route configuration might be used. Without the [Route("{id:int}")]
        //attribute, both Get methods will handle requests to the same URL (e.g., api/THEcontroller).
        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<CouponDto> GetById(int id)
        {
            try
            {
                // i use FirstOrDefault to avoid an exception if the coupon is not found
                Coupon foundCouponById = _db.Coupons.FirstOrDefault(x => x.CouponId == id);
                if (foundCouponById == null)
                {
                    return NotFound(); // Return 404 if the coupon is not found
                }

                CouponDto mappCouponToCouponDto = _mapper.Map<CouponDto>(foundCouponById);
                return Ok(mappCouponToCouponDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }



        [HttpGet]
        [Route("GetByCode/{code}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<CouponDto> GetByCode(string code)
        {
            try
            {
                Coupon foundCouponByCode = _db.Coupons.FirstOrDefault(x => x.CouponCode.ToLower() == code.ToLower());
                // If no coupon is found, return 404 Not Found
                if (foundCouponByCode == null)
                {
                    return NotFound();
                }
                else
                {
                    // Map the found coupon to the DTO and return it with 200 OK
                    CouponDto mappCouponToCouponDto = _mapper.Map<CouponDto>(foundCouponByCode);
                    return Ok(mappCouponToCouponDto);
                }
            }
            catch (Exception ex)
            {
                // Log the exception (if applicable) and return 500 Internal Server Error
                // Example: _logger.LogError(ex, "An error occurred while fetching the coupon by code");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the coupon.");

            }
        }




        //"FromBody" refers to the part of the request that contains the DATA(Create,Update, ...)
        //being sent from the CLINET(user) (like a browser, mobile app, or API client) to the server. 
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Post([FromBody] CouponDto couponDto)
        {
            if (couponDto == null)
            {
                return BadRequest("Invalid data.");
            }
            try
            {
                // Map CouponDto to Coupon entity
                Coupon createCoupon = _mapper.Map<Coupon>(couponDto);

                // Add new coupon to the database
                _db.Coupons.Add(createCoupon);
                _db.SaveChanges(); // Save changes to the database

                // Map the created Coupon back to CouponDto
                CouponDto mappCouponToCouponDto = _mapper.Map<CouponDto>(createCoupon);

                // Return 201 Created 
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                // Log the exception (optional) and return 500 Internal Server Error
                // Example: _logger.LogError(ex, "An error occurred while creating a coupon.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the coupon.");
            }
        }





        //[FromBody] = inputed data from user
        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Put([FromBody] CouponDto couponDto)
        {
            if (couponDto == null || couponDto.CouponId == 0) // Validate incoming data
            {
                return BadRequest("Invalid data.");
            }
            try
            {
                // Map CouponDto to Coupon entity
                Coupon updateCoupon = _mapper.Map<Coupon>(couponDto);

                // Update the coupon in the database
                _db.Coupons.Update(updateCoupon);
                _db.SaveChanges(); // Save changes to the database

                // Return 202 Accepted with no content (as PUT is typically used for updates)
                return StatusCode(StatusCodes.Status202Accepted);

            }
            catch (Exception ex)
            {
                // Log the exception (optional) and return 500 Internal Server Error
                // Example: _logger.LogError(ex, "An error occurred while updating the coupon.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the coupon.");
            }
        }





        //[FromBody] = inputed data from user
        [HttpDelete]
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid coupon ID.");
            }
            try
            {
                // Find the coupon by ID
                Coupon deleteCoupon = _db.Coupons.FirstOrDefault(x => x.CouponId == id);

                if (deleteCoupon == null)
                {
                    return NotFound($"Coupon with ID {id} not found.");
                }

                // Remove the coupon from the database
                _db.Coupons.Remove(deleteCoupon);
                _db.SaveChanges(); // Save changes to the database

                // Return 202 Accepted after successful deletion
                return StatusCode(StatusCodes.Status202Accepted);

            }
            catch (Exception ex)
            {
                // Log the exception (optional) and return 500 Internal Server Error
                // Example: _logger.LogError(ex, "An error occurred while deleting the coupon.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the coupon.");
            }
        }





        //Note
        //Swagger will automatically document the return types of the API endpoints.
        //Since the API endpoints return ResponseDto,


        //Note
        //Since the controller actions (Get() and GetById()) are returning ResponseDto,
        //Swagger will show the structure of ResponseDto and its fields.
    }
}

