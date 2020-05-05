namespace Core.Entities
{
    public class Product : BaseEntity
    {
        private decimal _price;
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get { return _price; } set { _price = value; SalePrice = value + (value / 10); } }
        public decimal SalePrice { get; private set; }
        public string PhotoUrl { get; set; }
        public ProductType ProductType { get; set; }
        public int ProductTypeId { get; set; }
        public ProductBrand ProductBrand { get; set; }
        public int ProductBrandId { get; set; }
    }
}