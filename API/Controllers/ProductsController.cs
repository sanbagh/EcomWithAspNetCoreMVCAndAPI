using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTO;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class ProductsController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public ProductsController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProductsAsync([FromQuery]ProductSpecParam productSpecParam)
        {
            ProductsWithBrandsAndTypesSpec spec = new ProductsWithBrandsAndTypesSpec(productSpecParam);
            ProuductsWithFilterCountSpec countProductsAfterFilter = new ProuductsWithFilterCountSpec(productSpecParam);
            int count = await _uow.Repository<Product>().GetCountAsync(countProductsAfterFilter);
            var productDto = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(await _uow.Repository<Product>().GetAllBySpecAsync(spec));
            Pagination<ProductToReturnDto> pagination = new Pagination<ProductToReturnDto>(productSpecParam.PageSize, productSpecParam.PageIndex, count, productDto);
            return Ok(pagination);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            ProductsWithBrandsAndTypesSpec spec = new ProductsWithBrandsAndTypesSpec(id);
            var product = _mapper.Map<Product, ProductToReturnDto>(await _uow.Repository<Product>().GetEntityBySpecAsync(spec));
            if (product == null) return NotFound(new ApiResponse(404));
            return product;
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductToReturnDto>> SaveProduct([FromBody]ProductDto productDto)
        {
            productDto.Id = 0;

            var brand = await _uow.Repository<ProductBrand>().GetByIdAsync(productDto.ProductBrandId);
            if (brand == null) return BadRequest(new ApiResponse(400, "Brand does not exists"));

            var type = await _uow.Repository<ProductType>().GetByIdAsync(productDto.ProductTypeId);
            if (type == null) return BadRequest(new ApiResponse(400, "Type does not exists"));

            var product = _mapper.Map<ProductDto, Product>(productDto);
            _uow.Repository<Product>().Add(product);
            int count = await _uow.Complete();

            if (count <= 0) return BadRequest(new ApiResponse(400, "Error occured while saving product"));

            product.ProductBrand = brand;
            product.ProductType = type;

            return _mapper.Map<Product, ProductToReturnDto>(product);
        }
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> DeleteProduct(int id)
        {
            var product = await _uow.Repository<Product>().GetByIdAsync(id);
            if (product == null) return NotFound(new ApiResponse(404));

            _uow.Repository<Product>().Delete(product);
            int count = await _uow.Complete();

            if (count <= 0) return BadRequest(new ApiResponse(400, "Error occured while deleting product"));
            return true;
        }
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductToReturnDto>> UpdateProduct([FromBody]ProductDto productDto)
        {
            var result = await _uow.Repository<Product>().GetByIdAsync(productDto.Id);

            if (result == null) return NotFound(new ApiResponse(404));

            var brand = await _uow.Repository<ProductBrand>().GetByIdAsync(productDto.ProductBrandId);
            if (brand == null) return BadRequest(new ApiResponse(400, "Brand does not exists"));

            var type = await _uow.Repository<ProductType>().GetByIdAsync(productDto.ProductTypeId);
            if (type == null) return BadRequest(new ApiResponse(400, "Type does not exists"));

            var product = _mapper.Map<ProductDto, Product>(productDto);

            _uow.Repository<Product>().Update(product);
            int count = await _uow.Complete();

            if (count <= 0) return BadRequest(new ApiResponse(400, "Error occured while updating product"));

            product.ProductBrand = brand;
            product.ProductType = type;

            return _mapper.Map<Product, ProductToReturnDto>(product);
        }
        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductToReturnDto>> UpdateProductPrice(int id, decimal price)
        {
            if (price <= 0) return BadRequest(new ApiResponse(400, "Price should be greater than 0"));

            ProductsWithBrandsAndTypesSpec spec = new ProductsWithBrandsAndTypesSpec(id);
            var product = await _uow.Repository<Product>().GetEntityBySpecAsync(spec);

            if (product == null) return NotFound(new ApiResponse(404));

            product.Price = price;
            _uow.Repository<Product>().Update(product);
            int count = await _uow.Complete();

            if (count <= 0) return BadRequest(new ApiResponse(400, "Error occured while updating product"));

            return _mapper.Map<Product, ProductToReturnDto>(product);
        }
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            return Ok(await _uow.Repository<ProductBrand>().GetAllAsync());
        }
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            return Ok(await _uow.Repository<ProductType>().GetAllAsync());
        }
    }
}

