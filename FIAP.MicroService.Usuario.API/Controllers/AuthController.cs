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
    private readonly ILogger<AuthController> _logger;

    public AuthController(IUserRepository userRepository, ITokenService tokenService, ILogger<AuthController> logger)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
    {
        var user = await _userRepository.FindByUsernameAndPasswordAsync(loginRequest.Username, loginRequest.Password);
        
        if (user == null)
        {
            _logger.LogInformation("Login inválido realizado | Username: {Username}", loginRequest.Username);

            return Unauthorized(new { message = "Usuário ou senha inválidos." });
        }

        var token = _tokenService.GenerateToken(user);

        _logger.LogInformation("Login realizado com sucesso | Username: {Username}", loginRequest.Username);

        // Se o usuário FOR encontrado, DEVE retornar 200 Ok.
        return Ok(new {
            token = token,
            userId = user.Id
        });
    }
}