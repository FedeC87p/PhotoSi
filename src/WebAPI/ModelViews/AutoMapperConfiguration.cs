using AutoMapper;
using DomainModel.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.ModelViews.Request;

namespace WebAPI.ModelViews
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<ProductCreateRequest, ProductDto>().ReverseMap();
            CreateMap<ProductUpdateRequest, ProductDto>().ReverseMap();

            CreateMap<CategoryCreateRequest, CategoryDto>().ReverseMap();
            CreateMap<CategoryUpdateRequest, CategoryDto>().ReverseMap();

            CreateMap<OptionCreateRequest, OptionDto>().ReverseMap();
            CreateMap<OptionUpdateRequest, OptionDto>().ReverseMap();


            CreateMap<OrderCreateRequest, OrderDto>()
                .ForMember(dest => dest.ProductItems,
                    opt => opt.MapFrom((src, dest) =>
                        src?.ProductItems?.Select(i => new OrderItemDto
                        {
                            ProductId = i.ProductId,
                            Quantity = i.Quantity,
                            OptionItems = i?.OptionItems?.Select(k => new OrderItemOptionDto
                            {
                                OptionId = k.OptionId,
                                Value = k.Value
                            })?.ToList()
                        }
                        )));
        }
    }
}
