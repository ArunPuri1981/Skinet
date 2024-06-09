using API.DTO;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDTO>()
            .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.BrandName))
            .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.TypeName))
            .ForMember(d=>d.PictureUrl, o=>o.MapFrom<ProductUrlResolver>());
        }
    }
}