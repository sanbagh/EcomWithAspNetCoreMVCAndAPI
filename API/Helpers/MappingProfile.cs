using API.DTO;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Core.OrderAggregate;

namespace API.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductToReturnDto>()
            .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
            .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
            .ForMember(d => d.PhotoUrl, o => o.MapFrom<ProductUrlResolver>());

            CreateMap<ProductDto, Product>();

            CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();
            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();
            CreateMap<AddressDto, Core.OrderAggregate.Address>();
            CreateMap<Order, OrderToReturnDto>()
            .ForMember(d => d.DeliveryMethod, o => o.MapFrom(x => x.DeliveryMethod.ShortName))
            .ForMember(d => d.ShippingPrice, o => o.MapFrom(x => x.DeliveryMethod.Price));

            CreateMap<OrderItem, OrderItemDto>()
            .ForMember(d => d.ProductItemId, o => o.MapFrom(x => x.ItemOrdered.ProductItemId))
            .ForMember(d => d.ProductName, o => o.MapFrom(x => x.ItemOrdered.ProductName))
            .ForMember(d => d.PhotoUrl, o => o.MapFrom<OrderItemUrlResolver>());
        }
    }
}