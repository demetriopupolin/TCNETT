using Core.Entity;
using Core.Input;   
using Core.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FiapCloudGamesApi.Controllers
{

    [ApiController]
    [Route("/[controller]")]
    public class AuthController(IConfiguration configuration, IUsuarioRepository usuarioRepository) : ControllerBase
    {
        private readonly IConfiguration _configuration = configuration;

        private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {

            // busca usuario pelo email
            var usuario = _usuarioRepository.ObterPorEmail(loginDto.Email);
                        
            if (usuario == null)
                return Unauthorized($"Usuário {loginDto.Email} não encontrado.");

            if (usuario.Senha != loginDto.Senha)
                return Unauthorized("Senha incorreta.");

            var role = usuario.Nivel == 'A' ? "Admin" : "User";
            var token = GenerateToken(usuario.Id, role);

            return Ok(new { token });
        }

        private string GenerateToken(int id, string role)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, id.ToString()),
            new Claim(ClaimTypes.Role, role),
            new Claim(ClaimTypes.NameIdentifier, id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // =======================
        // HEALTH CHECKS
        // =======================
        /// <summary>
        /// Endpoint de health check interno (retorna 200 OK).
        /// </summary>
        [HttpGet("/health")]
        public IActionResult Health()
        {
            return Ok("OK");
        }

        /// <summary>
        /// Endpoint usado por Load Balancer / Kubernetes.
        /// </summary>
        [HttpGet("/healthz")]
        public IActionResult Healthz()
        {
            return Ok("OK");
        }





        [HttpGet("/healthbd")]
        public IActionResult HealthBd()
        {
            var connectionString = _configuration.GetConnectionString("ConnectionString");

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("SELECT 1", connection))
                    {
                        command.ExecuteScalar();
                    }
                }

                return Ok(new { status = "UP", message = "Banco de dados acessível" });
            }
            catch (Exception ex)
            {
                return StatusCode(503, new { status = "DOWN", message = "Banco de dados indisponível", error = ex.Message });
            }
        }





    }

}
