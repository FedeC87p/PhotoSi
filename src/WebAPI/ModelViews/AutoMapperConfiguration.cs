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
        }
    }
}
