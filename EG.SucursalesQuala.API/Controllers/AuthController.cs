using Microsoft.AspNetCore.Mvc;
using EG.SucursalesQuala.Application.DTOs;
using EG.SucursalesQuala.Application.Interfaces;

namespace EG.SucursalesQuala.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            try
            {
                LoginResponse result = await _authService.AutenticarAsync(request);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Error durante la autenticación" });
            }
        }
    }
}