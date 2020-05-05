using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceDemo.Models
{
    public class BasketTotal
    {
        public decimal ShippingCharge { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
    }
}
