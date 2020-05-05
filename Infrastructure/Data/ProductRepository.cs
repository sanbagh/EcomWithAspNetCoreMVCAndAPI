using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data {
    public class ProductRepository : IProductRepository {
        private readonly IGenericRepository<Product> _repoProducts;
        public ProductRepository (IGenericRepository<Product> repoProducts) {
            _repoProducts = repoProducts;
        }
        public async Task<Product> GetProductByIdAsync (int id) {
            return await _repoProducts.GetByIdAsync(id);
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync () {
            return await _repoProducts.GetAllAsync();
        }
    }
}