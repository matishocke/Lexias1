using Azure.Messaging;
using Lexias.Web.Models;
using Lexias.Web.Service.IService;
using System.Net.Http;

namespace Lexias.Web.Service
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        public CouponService(IHttpClientFactory httpClientFactory, HttpClient httpClient)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = httpClient;
        }



        //GET ALL
        public async Task<IEnumerable<CouponDto>?> GetAllCouponAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("CouponAPI");
                var answer = await client.GetFromJsonAsync<IEnumerable<CouponDto>>("api/CouponAPI");
                return answer;
            }
            catch (HttpRequestException httpEx)
            {
                // Handle HTTP-specific exceptions here
                throw new Exception($"An error occurred when making the HTTP request: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                // Handle any other exceptions here
                throw new Exception($"An unexpected error occurred: {ex.Message}");
            }
        }





        //CREATE
        //here parameter are going to be user input 
        public async Task? CreateCouponAsync(CouponDto couponDto)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("CouponAPI");
                var response = await client.PostAsJsonAsync("api/CouponAPI", couponDto);

                if (!response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error creating coupon: {message}");
                }
            }
            catch (HttpRequestException httpEx)
            {
                // Handle HTTP-specific exceptions here
                throw new Exception($"An error occurred when making the HTTP request: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                // Handle any other exceptions here
                throw new Exception($"An unexpected error occurred: {ex.Message}");
            }
        }



        //UPDATE
        public async Task? UpdateCouponAsync(CouponDto couponDto)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("CouponAPI");
                var response = await client.PutAsJsonAsync("api/CouponAPI", couponDto);

                if (!response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }
            }
            catch (HttpRequestException httpEx)
            {
                // Handle HTTP-specific exceptions here
                throw new Exception($"An error occurred when making the HTTP request: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                // Handle any other exceptions here
                throw new Exception($"An unexpected error occurred: {ex.Message}");
            }
        }



        //DELETE
        public async Task? DeleteCouponAsync(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("CouponAPI");
                var response = await client.DeleteAsync($"api/CouponAPI/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error deleting coupon: {message}");
                }
            }
            catch (HttpRequestException httpEx)
            {
                // Handle HTTP-specific exceptions here
                throw new Exception($"An error occurred when making the HTTP request: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                // Handle any other exceptions here
                throw new Exception($"An unexpected error occurred: {ex.Message}");
            }
        }







        //GET ID
        public async Task<CouponDto?> GetCouponByIdAsync(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("CouponAPI");
                return await client.GetFromJsonAsync<CouponDto>($"api/CouponAPI/{id}");
            }
            catch (HttpRequestException httpEx)
            {
                // Handle HTTP-specific exceptions here
                throw new Exception($"An error occurred when making the HTTP request: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                // Handle any other exceptions here
                throw new Exception($"An unexpected error occurred: {ex.Message}");
            }
        }




        //GET CODE
        public async Task<CouponDto?> GetCouponCodeAsync(string couponCode)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                return await client.GetFromJsonAsync<CouponDto>($"api/CouponAPI/GetByCode/{couponCode}");
            }
            catch (HttpRequestException httpEx)
            {
                // Handle HTTP-specific exceptions here
                throw new Exception($"An error occurred when making the HTTP request: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                // Handle any other exceptions here
                throw new Exception($"An unexpected error occurred: {ex.Message}");
            }
        }


    }
}
