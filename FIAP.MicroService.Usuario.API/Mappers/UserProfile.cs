using AutoMapper;
using FIAP.MicroService.Usuario.API.DTOs;
using FIAP.MicroService.Usuario.Dominio.Entidades;

namespace FIAP.MicroService.Usuario.API.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserResponseDto>();
    }
}
