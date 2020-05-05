using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithBrandsAndTypesSpec : BaseSpecification<Product>
    {
        public ProductsWithBrandsAndTypesSpec(ProductSpecParam productParams)
        : base(x =>
                (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
                (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId)
                && (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId))
        {
            AddIncludes(x => x.ProductBrand);
            AddIncludes(x => x.ProductType);
            DecidingSortingOrder(productParams.Sort);
            AddPagination((productParams.PageSize * (productParams.PageIndex - 1)), productParams.PageSize);
        }
        public ProductsWithBrandsAndTypesSpec(int id) : base(x => x.Id == id)
        {
            AddIncludes(x => x.ProductBrand);
            AddIncludes(x => x.ProductType);
        }
        private void DecidingSortingOrder(string sort)
        {
            AddOrderBy(x => x.Name);
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort.ToLower())
                {
                    case "priceasc":
                        AddOrderBy(x => x.Price); break;
                    case "pricedesc":
                        AddOrderByDesc(x => x.Price); break;
                    default:
                        AddOrderBy(x => x.Name);
                        break;
                }
            }
        }
    }
}