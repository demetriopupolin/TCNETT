using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiapCloudGamesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class SecureController : ControllerBase
    {
        [HttpGet("admin")]
        [Authorize(Policy = "Admin")]
        public IActionResult AdminOnly()
        {
            return Ok("Este endpoint é acessível apenas por admin.");
        }

        [HttpGet("user")]
        [Authorize(Policy = "User")]
        public IActionResult AnyUser()
        {
            return Ok("Este endpoint é acessível apenas por usuário.");
        }

        [HttpGet("authenticated")]
        [Authorize(Policy = "Authenticated")]
        public IActionResult Authenticate()
        {
            return Ok("Este endpoint é acessível para usuario e administradores autenticados.");
        }
    }
}
