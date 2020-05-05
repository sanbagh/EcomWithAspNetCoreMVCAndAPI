using System.Threading.Tasks;
using API.DTO;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    public class BasketController : BaseApiController {
        private readonly IBasketRepo _repo;
        private readonly IMapper _mapper;
        public BasketController (IBasketRepo repo, IMapper mapper) {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasketAsync (string id) {
            var basket = await _repo.GetBasketAsync (id);
            return Ok (basket ?? new CustomerBasket (id));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdateBasketAsync (CustomerBasketDto basket) {
            var customerBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);
            var result = await _repo.CreateOrUpdateBasketAsync (customerBasket);
            return Ok (result);
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasketAsync (string id) {
            return Ok (await _repo.DeleteBasketAsync (id));
        }
    }
}