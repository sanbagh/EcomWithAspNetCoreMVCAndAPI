namespace Core.Specifications
{
    public class ProductSpecParam
    {
        const int Max_Page_Size = 50;
        public int PageIndex { get; set; } = 1;
        private int _pagesize = 6;
        private string _search;
        public int PageSize
        {
            get => _pagesize;
            set => _pagesize = (value > Max_Page_Size) ? Max_Page_Size : value;
        }
        public string Sort { get; set; }
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string Search { get => _search; set => _search = value.ToLower(); }
    }
}