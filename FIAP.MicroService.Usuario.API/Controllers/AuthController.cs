using Microsoft.AspNetCore.Mvc;
using FIAP.MicroService.Usuario.API.DTOs;
using FIAP.MicroService.Usuario.Dominio.Interfaces;

namespace FIAP.MicroService.Usuario.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public AuthController(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
    {
        var user = await _userRepository.FindByUsernameAndPasswordAsync(loginRequest.Username, loginRequest.Password);
        
        if (user == null)
        {
            // Se o usuário NÃO for encontrado, retorna 401 Unauthorized.
            return Unauthorized(new { message = "Usuário ou senha inválidos." });
        }

        var token = _tokenService.GenerateToken(user);

        // Se o usuário FOR encontrado, DEVE retornar 200 Ok.
        return Ok(new {
            token = token,
            userId = user.Id
        });
    }
}