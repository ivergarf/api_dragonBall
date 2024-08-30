using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
using Servicios.Api.Controllers;
using Servicios.Core.DTOs;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

[Route("api/[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly TokenService _tokenService;
    private readonly ILogger _logger;

    public TokenController(TokenService tokenService, ILogger<TokenController> logger)
    {
        _tokenService = tokenService;
        _logger = _logger;
    }

    [HttpPost("~/api/v1/Token/")]
    public IActionResult Login([FromBody] RequestToken model)
    {
        // validar las credenciales del usuario.
        if (model.Username == "api_dragonBall" && model.Password == "Nub3V0l4d0r4")
        {
            var token = _tokenService.GenerateToken(model.Username);
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
            var expiresIn = jwtToken.ValidTo - DateTime.UtcNow;

            var response = new RequestTokenDTO
            {
                Token = token,
                result = "OK",
                errors = "",
                status = 200,
                tokenType = "bearer",
                expiresIn = expiresIn.TotalSeconds.ToString("0")
                
            };

            return Ok(response);
        }
        else
        {
            return Unauthorized(new RequestTokenDTO
            {
                Token = "",
                result = "Error",
                errors = "Invalid username or password",
                status = 401,
                tokenType = "",
                expiresIn = "",

            });
        } 
       
    }
}

