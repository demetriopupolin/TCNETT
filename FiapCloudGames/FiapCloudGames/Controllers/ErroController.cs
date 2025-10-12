using Microsoft.AspNetCore.Mvc;

namespace FiapCloudGamesApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ErroController : ControllerBase
    {
        [HttpGet("simular")]
        public IActionResult SimularErro()
        {
            // Força uma exceção
            throw new Exception("Simulando um erro.");
        }
    }


}
