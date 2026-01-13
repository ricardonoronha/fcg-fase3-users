using AutoMapper;
using FIAP.MicroService.Usuario.API.DTOs;
using FIAP.MicroService.Usuario.Dominio.Interfaces;
using FIAP.MicroService.Usuario.Infraestrutura.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.MicroService.Usuario.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UsersController(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        // Usando o AutoMapper para converter a entidade para o DTO de resposta
        var userResponse = _mapper.Map<UserResponseDto>(user);

        return Ok(userResponse);
    }

    [HttpGet("{id}/email")]
    public async Task<IActionResult> GetUserEmail(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound(new { message = "Usuário não encontrado." });
        }
        return Ok(new { user.Email });
    }

    [HttpPost("RabbitMQ")]
    public async Task<IActionResult> PostRabbitMQ([FromServices] RabbitMQService rabbit)
    {
        await rabbit.EnviarDados(new { Texto = "Mensagem enviada pelo MicroServico de Usuarios!", Data = DateTime.Now });

        return Ok();
    }
}