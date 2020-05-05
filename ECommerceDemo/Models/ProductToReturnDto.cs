namespace ECommerceDemo.Models
{
    public class ProductToReturnDto
    {
        private decimal _price;
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get { return _price; } set { _price = value; SalePrice = value + value / 10; } }
        public string PhotoUrl { get; set; }
        public string ProductType { get; set; }
        public string ProductBrand { get; set; }
        public decimal SalePrice { get; set; }
    }
}