using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerceDemo.Models
{
    public class CustomerBasket
    {
        public CustomerBasket()
        {
            Id = Guid.NewGuid().ToString();
        }
        [Required]
        public string Id { get; set; }
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
    }
}