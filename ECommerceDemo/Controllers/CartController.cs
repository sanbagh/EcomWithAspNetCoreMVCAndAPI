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
    public class CartController : Controller
    {
        private Uri _baseAddress = new Uri("https://localhost:5001/api/");
        public async Task<IActionResult> IndexAsync()
        {
            return View(await GetCustomerBasketAsync());
        }
        private async Task UpdateCartAsync(CustomerBasket basket)
        {
            CustomerBasket basket1 = null;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = _baseAddress;
                var url = "basket";
                var json = JsonConvert.SerializeObject(basket);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using (var response = await client.PostAsync(url, content))
                {
                    try
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        basket1 = JsonConvert.DeserializeObject<CustomerBasket>(apiResponse);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("Error", ex.Message);
                    }
                }
            }
        }
        public async Task<IActionResult> IncrementQuantity(int id)
        {
            var basket = await GetCustomerBasketAsync();
            var item = basket.Items.FirstOrDefault(x => x.Id == id);

            if (item != null)
            {
                item.Quantity += 1;
                await UpdateCartAsync(basket);
                UpdateItemCount(basket);
            }
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> DecrementQuantity(int id)
        {
            var basket = await GetCustomerBasketAsync();
            var item = basket.Items.FirstOrDefault(x => x.Id == id);

            if (item != null)
            {
                if (item.Quantity > 1)
                {
                    item.Quantity -= 1;
                }
                else
                {
                    basket.Items.Remove(item);
                }
                await UpdateCartAsync(basket);
                UpdateItemCount(basket);
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> RemoveItem(int id)
        {
            var basket = await GetCustomerBasketAsync();
            var item = basket.Items.FirstOrDefault(x => x.Id == id);

            if (item != null)
                basket.Items.Remove(item);

            await UpdateCartAsync(basket);
            UpdateItemCount(basket);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> AddItemToCart(int id, int? quantity = 1)
        {
            var product = await GetProudct(id);
            if (product == null) return new BadRequestResult();
            BasketItem basketItem = new BasketItem
            {
                Id = product.Id,
                Brand = product.ProductBrand,
                Name = product.Name,
                PhotoUrl = product.PhotoUrl,
                Price = product.Price,
                Type = product.ProductType,
                Quantity = quantity.Value
            };
            var basket = await GetCustomerBasketAsync();
            AddOrUpdateItem(basketItem, basket, quantity.Value);
            await UpdateCartAsync(basket);
            UpdateItemCount(basket);
            return RedirectToAction("Index", "Home");
        }
        private void UpdateItemCount(CustomerBasket basket)
        {
            HttpContext.Session.SetInt32("itemCount", basket.Items.Count);
        }
        private async Task<ProductToReturnDto> GetProudct(int id)
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
            return result;
        }
        private void AddOrUpdateItem(BasketItem basketItem, CustomerBasket basket, int quantity)
        {
            var item = basket.Items.FirstOrDefault(x => x.Id == basketItem.Id);
            if (item == null)
                basket.Items.Add(basketItem);
            else
                item.Quantity += quantity;
        }

        private async Task<CustomerBasket> GetCustomerBasketAsync()
        {
            CustomerBasket basket = null;
            var id = HttpContext.Session.GetString("cartId");
            if (string.IsNullOrWhiteSpace(id))
            {
                basket = new CustomerBasket();
                HttpContext.Session.SetString("cartId", basket.Id);
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = _baseAddress;
                    var url = "basket?id=" + id;
                    using (var response = await client.GetAsync(url))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        basket = JsonConvert.DeserializeObject<CustomerBasket>(apiResponse);
                    }
                }
            }
            return basket;
        }
    }
}