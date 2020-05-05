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
    public class AccountController : Controller
    {
        private Uri _baseAddress = new Uri("https://localhost:5001/api/");
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync(Login login)
        {
            UserLocal user = null;
            if (ModelState.IsValid)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = _baseAddress;
                    var url = "account/login";
                    var json = JsonConvert.SerializeObject(login);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    using (var response = await client.PostAsync(url, content))
                    {
                        try
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            user = JsonConvert.DeserializeObject<UserLocal>(apiResponse);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("Error", ex.Message);
                            return View(login);
                        }
                    }
                }
            }
            if (user.Token != null)
            {
                HttpContext.Session.SetString("token", user.Token);
                return RedirectToAction("Index", "Home");
            }
            return View(login);
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(Register register)
        {
            UserLocal user = null;
            if (ModelState.IsValid)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = _baseAddress;
                    var url = "account/register";
                    var json = JsonConvert.SerializeObject(register);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    using (var response = await client.PostAsync(url, content))
                    {
                        try
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            user = JsonConvert.DeserializeObject<UserLocal>(apiResponse);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("Error", ex.Message);
                            return View(register);
                        }
                    }
                }
            }
            if (user != null && user.Token != null)
            {
                HttpContext.Session.SetString("token", user.Token);
                return RedirectToAction("Index", "Home");
            }
            return View(register);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}