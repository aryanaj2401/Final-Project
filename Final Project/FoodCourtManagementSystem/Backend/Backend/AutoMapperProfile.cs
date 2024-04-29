using AutoMapper;
using BackendUsers.DTO;
using BackendUsers.DTO.LoginDTOs;
using BackendUsers.Models;



namespace BackendUsers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<UserInputDTO, User>();
            CreateMap<User, UserOutputDTO>();
            CreateMap<LoginInputDTO, User>();
        }   
    }
}
