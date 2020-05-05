namespace Core.OrderAggregate
{
    public class ProductItemOrdered
    {
        public ProductItemOrdered()
        {
        }

        public ProductItemOrdered(int productItemId, string productName, string photoUrl)
        {
            this.ProductItemId = productItemId;
            this.ProductName = productName;
            this.PhotoUrl = photoUrl;

        }
        public int ProductItemId { get; set; }
        public string ProductName { get; set; }
        public string PhotoUrl { get; set; }
    }
}