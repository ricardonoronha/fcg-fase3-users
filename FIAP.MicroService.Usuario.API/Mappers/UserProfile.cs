using AutoMapper;
using FIAP.MicroService.Usuario.API.DTOs;
using FIAP.MicroService.Usuario.Dominio.Entidades; // Adicione este using para encontrar a classe User

namespace FIAP.MicroService.Usuario.API.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        // CORREÇÃO: Mapeia da entidade 'User' para o DTO 'UserResponseDto'
        CreateMap<User, UserResponseDto>();
    }
}