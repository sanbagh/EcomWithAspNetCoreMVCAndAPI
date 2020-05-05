using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ECommerceDemo.Models;
using System.Net.Http;
using System.Text.Json;
using Newtonsoft.Json;

namespace ECommerceDemo.Controllers
{
    public class HomeController : Controller
    {
        private Uri _baseAddress = new Uri("https://localhost:5001/api/");
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync(int? pageIndex = 1, string search = "")
        {
            Pagination<ProductToReturnDto> result = null;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = _baseAddress;
                var url = "products?pageIndex=" + pageIndex;
                if (!string.IsNullOrWhiteSpace(search))
                    url += "&search=" + search;

                using (var response = await client.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<Pagination<ProductToReturnDto>>(apiResponse);
                }
            }
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> GetSearchResult(int? pageIndex = 1, string search = "")
        {
            Pagination<ProductToReturnDto> result = null;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = _baseAddress;
                var url = "products?pageIndex=" + pageIndex;
                if (!string.IsNullOrWhiteSpace(search))
                    url += "&search=" + search;

                using (var response = await client.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<Pagination<ProductToReturnDto>>(apiResponse);
                }
            }
            return PartialView("Index", result);
        }
        public async Task<IActionResult> Details(int id)
        {
            ProductToReturnDto result = null;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = _baseAddress;
                var url = "products/" + id;
                using (var response = await client.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ProductToReturnDto>(apiResponse);
                }
            }
            return View(result);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
