using AutoMapper;
using DomainModel.Dtos;
using DomainModel.Entities.Products;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace PhotoSi.AC
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.OptionsId,
                    opt => opt.MapFrom((src, dest) =>
                        src?.Options?.Select(i=>i.OptionId)))
                .ForMember(dest => dest.CategoryId,
                    opt => opt.MapFrom((src, dest) =>
                        src?.CategoryId));

            CreateMap<Category, CategoryDto>();

            CreateMap<Option, OptionDto>();
        }
        
    }
}
