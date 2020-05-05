using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ECommerceDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ECommerceDemo.Controllers
{
    public class OrderController : Controller
    {
        private Uri _baseAddress = new Uri("https://localhost:5001/api/");
        public async Task<IActionResult> Index()
        {
            IReadOnlyList<OrderToReturn> ordersToReturn = null;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = _baseAddress;
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var url = "order";
                using (var response = await client.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ordersToReturn = JsonConvert.DeserializeObject<IReadOnlyList<OrderToReturn>>(apiResponse);
                }
            }
            return View(ordersToReturn);
        }
        public async Task<IActionResult> PlaceOrder()
        {
            var order = new Order
            {
                BasketId = HttpContext.Session.GetString("cartId"),
                DeliveryMethodId = 2,
                ShippingAddress = new Address
                {
                    City = "Agra",
                    Country = "India",
                    FirstName = Helper.GetDisplayName(HttpContext.Session.GetString("token")),
                    LastName = "Last",
                    Street = "101 street",
                    State = "UP",
                    ZipCode = "282002"
                }
            };

            OrderToReturn orderToReturn = null;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = _baseAddress;
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var url = "order";
                var json = JsonConvert.SerializeObject(order);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using (var response = await client.PostAsync(url, content))
                {
                    try
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        orderToReturn = JsonConvert.DeserializeObject<OrderToReturn>(apiResponse);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("Error", ex.Message);
                    }
                }
            }
            if (HttpContext.Session.GetString("cartId") != null)
                HttpContext.Session.Remove("cartId");
            if (HttpContext.Session.GetInt32("itemCount") != null)
                HttpContext.Session.Remove("itemCount");
            return View(orderToReturn);
        }
        public async Task<IActionResult> ViewOrder(int id)
        {
            OrderToReturn orderToReturn = null;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = _baseAddress;
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var url = "order/" + id;
                using (var response = await client.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    orderToReturn = JsonConvert.DeserializeObject<OrderToReturn>(apiResponse);
                }
            }
            return View(orderToReturn);
        }
    }
}