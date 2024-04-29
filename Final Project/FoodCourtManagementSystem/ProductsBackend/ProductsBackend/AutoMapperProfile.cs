using AutoMapper;
using ProductsBackend.DTO;

using ProductsBackend.Models;

namespace ProductsBackend
{
    public class AutoMapperProfile :Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CategoriesInputDTO, Category>();
            CreateMap<ProductInputDTO, Product>();

        }
        
    }
}
